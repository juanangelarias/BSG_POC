using System.ComponentModel.DataAnnotations;
using BSG.Common.DTO;
using BSG.Common.DTO.Base;

namespace BSG.Common;

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
    
    [Required(ErrorMessage = "Display Name is required")]
    [MaxLength(100, ErrorMessage = "Display Name cannot exceed 200 characters")]
    public string DisplayName { get; set; } = "";
    
    [MaxLength(200, ErrorMessage = "Display Name cannot exceed 200 characters")]
    public string Tooltip { get; set; } = "";
    
    [MaxLength(500, ErrorMessage = "Help cannot exceed 500 characters")]
    public string Help { get; set; } = "";

    public ComponentDto Component { get; set; } = null!;
}