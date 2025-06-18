using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ProfileDto: DtoBase
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}