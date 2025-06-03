namespace BSG.BackEnd.Common.Model;

public class PasswordSettings
{
    public int PasswordExpirationInDays { get; set; }
    public int WelcomeTokenExpirationInHours { get; set; }
    public int ResetPasswordTokenExpirationInMinutes { get; set; }
    public int PasswordHistory { get; set; }
}