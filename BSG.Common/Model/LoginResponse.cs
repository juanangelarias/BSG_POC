using BSG.Common.DTO;

namespace BSG.Common.Model;

public class LoginResponse
{
    public UserDto? User { get; set; }
    public string Token { get; set; } = string.Empty;
}