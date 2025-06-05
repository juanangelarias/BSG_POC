using System.ComponentModel.DataAnnotations;
using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ProductDto : DtoBase
{
    [Required(ErrorMessage = "The 'Code' field is required")]
    [MaxLength(50, ErrorMessage = "The maximum length is 50 characters")]
    public string Code { get; set; } = "";

    [Required(ErrorMessage = "The 'Name' field is required")]
    [MaxLength(100, ErrorMessage = "The maximum length is 100 characters")]
    public string Name { get; set; } = "";

    [MaxLength(500, ErrorMessage = "The maximum length is 500 characters")]
    public string Description { get; set; } = "";

    [Required]
    public long ProductTypeId { get; set; }

    public ProductTypeDto ProductType { get; set; } = null!;
}