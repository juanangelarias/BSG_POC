using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ElementLanguageDto: DtoBase
{
    public long ElementId { get; set; }
    public long LanguageId { get; set; }
    public string DisplayName { get; set; } = "";
    public string Tooltip { get; set; } = "";
    public string Help { get; set; } = "";
    
    //
    
    public ElementDto Element { get; set; } = null!;
    public LanguageDto Language { get; set; } = null!;
}