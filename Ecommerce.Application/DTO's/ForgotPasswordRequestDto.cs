using System.ComponentModel.DataAnnotations;

namespace SimpleCRUDAPI.Ecommerce.Application.DTO_s
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}