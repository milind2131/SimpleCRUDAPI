namespace SimpleCRUDAPI.Ecommerce.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool IsEmailVerified { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
