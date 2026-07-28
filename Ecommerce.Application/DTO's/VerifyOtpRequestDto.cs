using ECommerce.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleCRUDAPI.Ecommerce.Application.DTO_s
{
    public class VerifyOtpRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6)]
        public string OTP { get; set; } = string.Empty;

        public OtpPurpose Purpose { get; set; }
    }
}