using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class UserProfileDto: DtoBase
{
    public long UserId { get; set; }
    public long ProfileId { get; set; }

    public UserDto User { get; set; } = null!;
    public ProfileDto Profile { get; set; } = null!;
}