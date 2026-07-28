using ECommerce.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.Auth
{
    public class ResendOtpRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        public OtpPurpose Purpose { get; set; }
    }
}