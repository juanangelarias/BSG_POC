using System.ComponentModel.DataAnnotations;
using BSG.Common.DTO.Base;

namespace BSG.Common.DTO;

public class ProductTypeDto : DtoBase
{
    [Required(ErrorMessage = "The 'Name' field is required")]
    [MaxLength(50, ErrorMessage = "The maximum length is 50 characters")]
    public string Name { get; set; } = "";

    [MaxLength(500, ErrorMessage = "The maximum length is 500 characters")]
    public string Description { get; set; } = "";

    public long ProductTypeId { get; set; }
}