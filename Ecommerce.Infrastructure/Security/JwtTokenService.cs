using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Configurations;
using SimpleCRUDAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SimpleCRUDAPI.Ecommerce.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public (string Token, DateTime Expiration) GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.UniqueName, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Email, user.Email),
              // Role Claim
            new(ClaimTypes.Role, user.RoleName)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return
        (
            new JwtSecurityTokenHandler().WriteToken(token),
            expiration
        );
    }

    //public string GenerateAccessToken(int userId, string email, string role)
    //{
    //    var claims = new[]
    //    {
    //            new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
    //            new Claim(ClaimTypes.Email,email),
    //            new Claim(ClaimTypes.Role,role)
    //        };

    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

    //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    var token = new JwtSecurityToken(
    //        issuer: _jwtSettings.Issuer,
    //        audience: _jwtSettings.Audience,
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
    //        signingCredentials: credentials);

    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }
}