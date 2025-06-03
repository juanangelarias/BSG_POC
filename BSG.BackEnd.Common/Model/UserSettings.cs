namespace BSG.BackEnd.Common.Model;

public class UserSettings
{
    public int WelcomeEmailTokenExpirationInMinutes { get; set; }
    public int ResetPasswordEmailTokenExpirationInMinutes { get; set; }
}