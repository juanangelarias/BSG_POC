using System.ComponentModel.DataAnnotations;
using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ComponentDto: DtoBase
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string Description { get; set; } = "";

    public List<ElementDto> Elements { get; set; } = [];
}