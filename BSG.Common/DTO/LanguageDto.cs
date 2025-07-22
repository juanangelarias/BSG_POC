using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class LanguageDto: DtoBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}