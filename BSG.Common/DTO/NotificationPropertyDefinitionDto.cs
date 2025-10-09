using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class NotificationPropertyDefinitionDto: DtoBase
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
}