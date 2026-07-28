namespace SimpleCRUDAPI.Ecommerce.Domain.Entities
{
    public class PasswordResetRequest
    {
        public int PasswordResetRequestId { get; set; }

        public int UserId { get; set; }

        public string OTPHash { get; set; } = string.Empty;

        public DateTime OTPExpiry { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}