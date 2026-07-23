using Dapper;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Constants;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using SimpleCRUDAPI.Model;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuthRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> RegisterUserAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            StoredProcedures.RegisterUser,
            new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.MobileNumber,
                user.PasswordHash
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            StoredProcedures.GetUserByEmail,
            new
            {
                Email = email
            },
            commandType: CommandType.StoredProcedure);
    }
}