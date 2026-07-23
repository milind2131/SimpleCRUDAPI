using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces
{
    public interface IJwtTokenService
    {
        (string Token, DateTime Expiration) GenerateToken(User user);
    }
}
