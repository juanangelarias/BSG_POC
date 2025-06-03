using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class UserPasswordDto: DtoBase
{
    public long UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public byte[]? Key { get; set; }
    public byte[]? Password { get; set; }
    
    //
    
    public UserDto? User { get; set; }
}