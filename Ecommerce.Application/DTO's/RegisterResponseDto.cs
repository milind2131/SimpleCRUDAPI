namespace SimpleCRUDAPI.Ecommerce.Application.DTO_s
{
    public class RegisterResponseDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
