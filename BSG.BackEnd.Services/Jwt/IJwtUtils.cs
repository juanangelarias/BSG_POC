using BSG.Common.DTO;

namespace BSG.BackEnd.Services.Jwt;

public interface IJwtUtils
{
    string GenerateJwtToken(UserDto user );
    long? ValidateJwtToken( string? token );
}