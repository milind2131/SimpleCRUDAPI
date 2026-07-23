using System.ComponentModel.DataAnnotations;

namespace SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}