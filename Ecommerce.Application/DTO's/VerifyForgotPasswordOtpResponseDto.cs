namespace SimpleCRUDAPI.Ecommerce.Application.DTO_s
{
    public class VerifyForgotPasswordOtpResponseDto
    {
        public bool IsVerified { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}