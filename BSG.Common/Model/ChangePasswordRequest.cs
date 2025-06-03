namespace BSG.Common.Model;

public class ChangePasswordRequest
{
    public string Token { get; set; } = string.Empty;
    public bool IsNewUser { get; set; }
    public string? OldPassword { get; set; }
    public string NewPassword { get; set; } = string.Empty;
}