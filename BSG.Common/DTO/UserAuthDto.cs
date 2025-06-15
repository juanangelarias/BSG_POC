using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class UserAuthDto: DtoBase
{
    public long UserId { get; set; }
    public long ElementId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsVisible { get; set; }

    public UserDto User { get; set; } = null!;
    public ElementDto Element { get; set; } = null!;
}