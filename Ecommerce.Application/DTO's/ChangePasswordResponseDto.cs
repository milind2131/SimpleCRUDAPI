namespace ECommerce.API.DTOs.Auth
{
    public class ChangePasswordResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}