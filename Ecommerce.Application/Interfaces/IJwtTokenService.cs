using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using System.Security.Claims;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces
{
    public interface IJwtTokenService
    {
        (string Token, DateTime Expiration) GenerateToken(User user);

       // string GenerateAccessToken(int userId, string email, string role);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
