using Dapper;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Constants;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using System.Data;

namespace SimpleCRUDAPI.Ecommerce.Infrastructure.Repositories;

public class RoleUpgradeRequestRepository
    : IRoleUpgradeRequestRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RoleUpgradeRequestRepository(
        IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(
        int userId,
        string requestedRole)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            RoleUpgradeStoredProcedures.Create,
            new
            {
                UserId = userId,
                RequestedRole = requestedRole
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<RoleUpgradeRequest?>
        GetLatestByUserIdAsync(
            int userId)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<RoleUpgradeRequest>(
                RoleUpgradeStoredProcedures.GetLatestByUserId,
                new
                {
                    UserId = userId
                },
                commandType:
                    CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<RoleUpgradeRequest>>
        GetPendingAsync()
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection
            .QueryAsync<RoleUpgradeRequest>(
                RoleUpgradeStoredProcedures.GetPending,
                commandType:
                    CommandType.StoredProcedure);
    }

    public async Task<int> ApproveAsync(
        int requestId,
        int reviewedByUserId,
        string? remarks)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            RoleUpgradeStoredProcedures.Approve,
            new
            {
                RequestId = requestId,
                ReviewedByUserId = reviewedByUserId,
                ReviewRemarks = remarks
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> RejectAsync(
        int requestId,
        int reviewedByUserId,
        string? remarks)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            RoleUpgradeStoredProcedures.Reject,
            new
            {
                RequestId = requestId,
                ReviewedByUserId = reviewedByUserId,
                ReviewRemarks = remarks
            },
            commandType: CommandType.StoredProcedure);
    }
}