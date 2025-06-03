using BSG.BackEnd.Common.Model;
using BSG.BackEnd.Services.Encryption;
using BSG.BackEnd.Services.Jwt;
using BSG.BackEnd.Services.Mail;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Repository;

namespace BSG.Features;

public interface IUserFeature
{
    Task<LoginResponse> Login( LoginRequest request );
    Task SendWelcomeEmail( long userId );
    Task SendForgotPasswordEmail( string username );
    Task SendPasswordChangedConfirmationEmail( long userId );
    Task<bool> ChangePassword(long userId, ChangePasswordRequest request);
    Task<bool> ResetPassword( long userId, string password );
}

public class UserFeature(IUserRepository userRepository,
    IUserPasswordRepository userPasswordRepository,
    IEncryptionService encryptionService,
    IJwtUtils jwtUtils,
    IMailService mailService,
    FrontEndParameters frontEndParameters,
    PasswordSettings passwordSettings,
    UserSettings userSettings)
{
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var response = new LoginResponse
        {
            User = null,
            Token = string.Empty
        };

        var user = await userRepository.GetByUsername(request.Username);
        if (user is not {IsEnabled: true})
            return response;
        

        var keyPass = await userPasswordRepository.GetUserKeyPassword(user.Id);
        if (keyPass == null)
            return response;

        var pass = keyPass.Value.Password ?? [];
        var key = keyPass.Value.Key ?? [];

        var valid = encryptionService.VerifyHashed(request.Password, pass, key);

        if (!valid)
            return response;

        response.User = user;
        response.Token = jwtUtils.GenerateJwtToken(user);

        return response;
    }

    public async Task SendWelcomeEmail(long userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user.Username == null)
            return;

        var token = await GetToken(EmailType.Welcome, user.Id);

        var logoUrl = $"{frontEndParameters.BaseUrl}/resources/logo.jpg";
        var linkUrl =
            $"{frontEndParameters.BaseUrl}/{frontEndParameters.WelcomeComponent}?username={user.Username}&token={token}";

        var template = EmailTemplates.WelcomeTemplate
            .Replace("&&LOGO_URL&&", logoUrl)
            .Replace("&&FULL_NAME&&", user.FullName)
            .Replace("&&EMAIL_ADDRESS&&", user.Email)
            .Replace("&&LINK_URL&&", linkUrl);

        var message = new EmailMessage()
        {
            Body = template,
            IsHtml = true,
            Subject = "Welcome to the Conference Manager App.",
            To = new List<string> {user.Email!}
        };

        mailService.SendEmail(message);
    }

    public async Task SendForgotPasswordEmail(string username)
    {
        var user = await userRepository.GetByUsername(username);
        if (user?.Username == null)
            return;

        var token = await GetToken(EmailType.ResetPassword, user.Id);

        var logoUrl = $"{frontEndParameters.BaseUrl}/resources/logo.jpg";
        var linkUrl =
            $"{frontEndParameters.BaseUrl}/{frontEndParameters.ForgotPasswordComponent}?username={user.Username}&token={token}";

        var template = EmailTemplates.WelcomeTemplate
            .Replace("&&LOGO_URL&&", logoUrl)
            .Replace("&&FULL_NAME&&", user.FullName)
            .Replace("&&BASE_URL&&", frontEndParameters.BaseUrl)
            .Replace("&&LINK_URL&&", linkUrl);

        var message = new EmailMessage()
        {
            Body = template,
            IsHtml = true,
            Subject = "Conference Manager App. Reset password",
            To = new List<string> {user.Email!}
        };

        mailService.SendEmail(message);
    }

    public async Task SendPasswordChangedConfirmationEmail(long userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user.Username == null)
            return;

        var logoUrl = $"{frontEndParameters.BaseUrl}/resources/logo.jpg";
        var linkUrl = $"{frontEndParameters.BaseUrl}/{frontEndParameters.PasswordChangeEmailConfirmation}";

        var template = EmailTemplates.WelcomeTemplate
            .Replace("&&LOGO_URL&&", logoUrl)
            .Replace("&&FULL_NAME&&", user.Username)
            .Replace("&&BASE_URL&&", frontEndParameters.BaseUrl)
            .Replace("&&LINK_URL&&", linkUrl);

        var message = new EmailMessage()
        {
            Body = template,
            IsHtml = true,
            Subject = "Conference Manager App. Password changed confirmation",
            To = new List<string> {user.Email!}
        };

        mailService.SendEmail(message);
    }

    public async Task<bool> ChangePassword(long userId, ChangePasswordRequest request)
    {
        var valid = await userRepository.ValidateEmailToken(userId, request.Token);

        if (!valid)
            return false;

        if (!request.IsNewUser)
        {
            if (string.IsNullOrEmpty(request.OldPassword))
                return false;

            var oldPasswordIsValid = await userPasswordRepository.ValidatePassword(userId, request.OldPassword);
            if (!oldPasswordIsValid)
                return false;
        }

        var now = DateTime.UtcNow;
        var end = now.AddDays(passwordSettings.PasswordExpirationInDays);
        var key = encryptionService.CreateSalt();
        var hashed = encryptionService.OneWayEncrypt(request.NewPassword, key);
        var userPassword = new UserPasswordDto
        {
            UserId = userId,
            StartDate = now,
            EndDate = end,
            Key = key,
            Password = hashed
        };
        await userPasswordRepository.CreateAsync(userPassword);
        await userRepository.SetEmailToken(userId, string.Empty, null);
        await SendPasswordChangedConfirmationEmail(userId);

        return true;
    }

    public async Task<bool> ResetPassword(long userId, string password)
    {
        var start = DateTime.UtcNow;
        var end = start.AddDays(passwordSettings.PasswordExpirationInDays);
        var salt = encryptionService.CreateSalt();
        var hashed = encryptionService.OneWayEncrypt(password, salt);

        var usedPreviously = await userPasswordRepository.VerifyIfPasswordWasUsed(userId, hashed);
        if (usedPreviously)
            return false;

        await userPasswordRepository.CreateAsync(new UserPasswordDto
        {
            UserId = userId,
            StartDate = start,
            EndDate = end,
            Key = salt,
            Password = hashed
        });

        return true;
    }

    private async Task<string> GetToken(EmailType type, long userId)
    {
        var token = encryptionService.CreateMailToken();
        var expire = type switch
        {
            EmailType.Welcome => DateTime.UtcNow.AddMinutes(userSettings.WelcomeEmailTokenExpirationInMinutes),
            EmailType.ResetPassword => DateTime.UtcNow.AddMinutes(userSettings
                .ResetPasswordEmailTokenExpirationInMinutes),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        await userRepository.SetEmailToken(userId, token, expire);

        return token;
    }

    private enum EmailType
    {
        Welcome,
        ResetPassword
    }
}