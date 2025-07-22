using System.ComponentModel.DataAnnotations;
using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ElementDto: DtoBase
{
    [Required(ErrorMessage = "Component Id is required")]
    public long ComponentId { get; set; }
    
    [Required(ErrorMessage = "Code is required")]
    [MaxLength(100, ErrorMessage = "Code cannot exceed 100 characters")]
    public string Code { get; set; } = "";
    
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = "";
    
    public List<ElementLanguageDto> Languages { get; set; } = [];
}