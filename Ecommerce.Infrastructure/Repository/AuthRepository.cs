using Dapper;
using ECommerce.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Constants;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using SimpleCRUDAPI.Model;
using System.Data;
using System.Data.Common;

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

    public async Task<bool> ChangePasswordAsync( int userId,string passwordHash)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<bool>(
            StoredProcedures.ChangePassword,
            new
            {
                UserId = userId,
                PasswordHash = passwordHash
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            StoredProcedures.GetUserById,
            new
            {
                UserId = userId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertPendingUserAsync(User user, string otpHash, DateTime otpExpiry)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            StoredProcedures.InsertPendingUser,
            new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.MobileNumber,
                user.PasswordHash,
                OTPHash = otpHash,
                OTPExpiry = otpExpiry
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<PendingUser?> GetPendingUserByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<PendingUser>(
            StoredProcedures.GetPendingUserByEmail,
            new
            {
                Email = email
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeletePendingUserAsync(int pendingUserId)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            StoredProcedures.DeletePendingUser,
            new
            {
                PendingUserId = pendingUserId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdateRegistrationOtpAsync( string email,string otpHash,DateTime otpExpiry)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            StoredProcedures.UpdateRegistrationOtp,
            new
            {
                Email = email,
                OTPHash = otpHash,
                OTPExpiry = otpExpiry
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertPasswordResetRequestAsync(
    int userId,
    string otpHash,
    DateTime otpExpiry)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            StoredProcedures.InsertPasswordResetRequest,
            new
            {
                UserId = userId,
                OTPHash = otpHash,
                OTPExpiry = otpExpiry
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<PasswordResetRequest?> GetPasswordResetRequestAsync(
    int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<PasswordResetRequest>(
            StoredProcedures.GetPasswordResetRequest,
            new
            {
                UserId = userId
            },
            commandType: CommandType.StoredProcedure);
    }
    public async Task VerifyPasswordResetRequestAsync(
    int passwordResetRequestId)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            StoredProcedures.VerifyPasswordResetRequest,
            new
            {
                PasswordResetRequestId = passwordResetRequestId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeletePasswordResetRequestAsync(
    int passwordResetRequestId)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            StoredProcedures.DeletePasswordResetRequest,
            new
            {
                PasswordResetRequestId = passwordResetRequestId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> UpdatePasswordResetOtpAsync(
    int passwordResetRequestId,
    string otpHash,
    DateTime otpExpiry)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            StoredProcedures.UpdatePasswordResetOtp,
            new
            {
                PasswordResetRequestId = passwordResetRequestId,
                OTPHash = otpHash,
                OTPExpiry = otpExpiry
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdatePasswordAsync(
    int userId,
    string passwordHash)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            StoredProcedures.UpdatePassword,
            new
            {
                UserId = userId,
                PasswordHash = passwordHash
            },
            commandType: CommandType.StoredProcedure);
    }

    // AuthRepository.cs

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();

        parameters.Add("@UserId", refreshToken.UserId);
        parameters.Add("@RefreshToken", refreshToken.Token);
        parameters.Add("@ExpiryDate", refreshToken.ExpiryDate);
        parameters.Add("@CreatedByIp", refreshToken.CreatedByIp);

        await connection.ExecuteAsync(
            StoredProcedures.SaveRefreshToken,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();

        parameters.Add("@RefreshToken", refreshToken);

        return await connection.QueryFirstOrDefaultAsync<RefreshToken>(
            StoredProcedures.GetRefreshToken,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, string? replacedByToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();

        parameters.Add("@RefreshToken", refreshToken);
        parameters.Add("@ReplacedByToken", replacedByToken);

        await connection.ExecuteAsync(
            StoredProcedures.RevokeRefreshToken,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task RevokeAllRefreshTokensByUserIdAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();

        parameters.Add("@UserId", userId);

        await connection.ExecuteAsync(
            StoredProcedures.RevokeAllRefreshTokensByUserId,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

}