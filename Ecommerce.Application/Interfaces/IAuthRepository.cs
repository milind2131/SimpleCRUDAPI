using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IAuthRepository
{
    Task<int> RegisterUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);
}

