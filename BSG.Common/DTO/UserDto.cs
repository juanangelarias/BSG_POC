using System.ComponentModel.DataAnnotations;
using BSG.Common.DTO.Base;
using BSG.Common.Model;

namespace BSG.Common.DTO;

public class UserDto: DtoBase
{
    [Required( ErrorMessage = "The 'Username' field is required" )]
    [MaxLength( 250, ErrorMessage = "The maximum length is 250 characters" )]
    [RegularExpression( Constants.RegExpEmail, ErrorMessage = "The 'Username' field must be in form of an email address" )]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "The 'Full Name' field is required")]
    [MaxLength(100, ErrorMessage = "The maximum length is 100 characters")]
    public string? FullName { get; set; }

    [Required( ErrorMessage = "The 'Email' field is required" )]
    [MaxLength( 250, ErrorMessage = "The maximum length is 250 characters" )]
    [RegularExpression( Constants.RegExpEmail, ErrorMessage = "The 'Email' field is not a valid email address." )]
    public string? Email { get; set; }

    [MaxLength( 20, ErrorMessage = "The maximum length is 20 characters" )]
    [RegularExpression(Constants.RegExpPhone, ErrorMessage = "This is not a phone number")]
    public string? PhoneNumber { get; set; }

    [Required( ErrorMessage = "The 'Mobile number' field is required" )]
    [MaxLength( 20, ErrorMessage = "The maximum length is 20 characters" )]
    [RegularExpression(Constants.RegExpPhone, ErrorMessage = "This is not a phone number")]
    public string? MobileNumber { get; set; }

    public bool IsEnabled { get; set; }
    public bool IsEmailConfirmed { get; set; }
    
    [Required( ErrorMessage = "The 'Is Admin' field is required" )]
    public bool IsAdmin { get; set; }
}