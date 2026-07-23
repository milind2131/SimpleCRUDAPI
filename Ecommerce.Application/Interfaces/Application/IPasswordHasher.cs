namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces.Application
{
    public interface IPasswordHasher
    {
        (string PasswordHash, string PasswordSalt) HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash, string passwordSalt);
    }
}
