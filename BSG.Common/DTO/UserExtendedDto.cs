using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class UserExtendedDto: UserDto
{
    public List<UserAuthDto> UserAuths { get; set; } = [];
    public List<UserProfileDto> UserProfiles { get; set; } = [];
}