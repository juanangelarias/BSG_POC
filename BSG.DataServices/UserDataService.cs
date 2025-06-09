using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BSG.Common.DTO;
using BSG.Common.Exceptions;
using BSG.Common.Model;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;
using Constants = BSG.States.Model.FrontEndConstants;

namespace BSG.DataServices;

public interface IUserDataService: IDataServiceBase<UserDto>
{
    Task SendWelcomeEmail( long userId );
    Task SendForgotPasswordEmail( long userId );
    Task SendPasswordChangedConfirmation( long userId );
    Task<LoginResponse?> Authenticate( LoginRequest request );
    Task<bool?> VerifyEmailExistence( long userId, string email );
    Task<UserDto?> GetByUsernameAndEmailToken(string username, string token);
    Task<bool> SetPassword(long userId, ChangePasswordRequest request);
    Task<UserDto?> GetByUserName(string username);
}

public class UserDataService : DataServiceBase<UserDto>, IUserDataService
{
    public UserDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/user";
    }

    public async Task SendWelcomeEmail(long userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{userId}/sendWelcomeEmail");

        await GetResponse(request);
    }

    public async Task SendForgotPasswordEmail(long userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{userId}/sendForgotPasswordEmail");

        await GetResponse(request);
    }

    public async Task SendPasswordChangedConfirmation(long userId)
    {
        var request =
            new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{userId}/sendPasswordChangedConfirmationEmail");

        await GetResponse(request);
    }

    public async Task<LoginResponse?> Authenticate(LoginRequest loginRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/authenticate");
        request.Content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            Constants.MediaType);

        var response = await GetResponse(request);

        if (response == null)
            return null;

        var result = await response.Content.ReadFromJsonAsync<Response<LoginResponse>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return !result.Success 
            ? throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later") 
            : result.Content;
    }

    public async Task<bool?> VerifyEmailExistence(long userId, string email)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{userId}/verifyEmailExistence");
        request.Content = new StringContent(
            JsonSerializer.Serialize(email),
            Encoding.UTF8,
            Constants.MediaType);

        var response = await GetResponse(request);

        if (response == null)
            return null;

        var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        if (!result.Success)
            throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later");

        return result.Content;
    }

    public async Task<UserDto?> GetByUsernameAndEmailToken(string username, string token)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/getByUsernameAndEmailToken/{username}/{token}");

        var response = await GetResponse(request);

        if (response == null)
            return null;

        var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        if (!result.Success)
            throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later");

        return result.Content;
    }

    public async Task<bool> SetPassword(long userId, ChangePasswordRequest passwordRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/{userId}/setPassword");
        request.Content = new StringContent(
            JsonSerializer.Serialize(passwordRequest),
            Encoding.UTF8,
            Constants.MediaType);

        var response = await GetResponse(request);

        if (response == null)
            return false;

        var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        if (!result.Success)
            throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later");

        return result.Content;
    }

    public async Task<UserDto?> GetByUserName(string username)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/getByUsername/{username}");

        var response = await GetResponse(request);

        if (response == null)
            return null;

        var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        if (!result.Success)
            throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later");

        return result.Content;
    }
}