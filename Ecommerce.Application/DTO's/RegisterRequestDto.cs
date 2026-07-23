using System.ComponentModel.DataAnnotations;

namespace SimpleCRUDAPI.Ecommerce.Application.DTO_s
{
    public class RegisterRequestDto
    {
            [Required]
            [StringLength(100)]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [StringLength(255)]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Phone]
            [StringLength(15)]
            public string MobileNumber { get; set; } = string.Empty;
        
            [Required]
            [MinLength(8)]
            public string Password { get; set; } = string.Empty;
        
    }
}
