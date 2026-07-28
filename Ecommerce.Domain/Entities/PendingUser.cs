namespace SimpleCRUDAPI.Ecommerce.Domain.Entities
{
    public class PendingUser
    {
        public int PendingUserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string OTPHash { get; set; } = string.Empty;

        public DateTime OTPExpiry { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}