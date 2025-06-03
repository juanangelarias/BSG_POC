using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BSG.BackEnd.Common.Model;
using BSG.Common.DTO;
using BSG.Common.Model;
using Microsoft.IdentityModel.Tokens;


namespace BSG.BackEnd.Services.Jwt;

public class JwtUtils : IJwtUtils
{
    private readonly JwtSettings _jwtSettings;

    public JwtUtils(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;

        if (string.IsNullOrEmpty(_jwtSettings.Key))
            throw new Exception("JWT Key (secret) not configured.");
    }

    public string GenerateJwtToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(GetClaims(user)),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenValidityInMinutes ?? 1440),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public long? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key!);

        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userId = long.Parse(jwtToken.Claims.First(f => f.Type == "id").Value);

            return userId;
        }
        catch
        {
            return null;
        }
    }

    private static IEnumerable<Claim> GetClaims(UserDto user)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Username!),
            new(ClaimTypes.Email, user.Email!)
        };

        if (user.IsAdmin)
            claims.Add(new Claim(ClaimTypes.Role, Constants.RoleAdmin));
        
        return claims;
    }
}