using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ProfileAuthDto: DtoBase
{
    public long ProfileId { get; set; }
    public long ElementId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsVisible { get; set; }

    public ProfileDto Profile { get; set; } = null!;
    public ElementDto Element { get; set; } = null!;
}