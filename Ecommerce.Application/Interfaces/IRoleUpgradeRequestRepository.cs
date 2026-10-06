using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IRoleUpgradeRequestRepository
{
    Task<int> CreateAsync(
        int userId,
        string requestedRole);

    Task<RoleUpgradeRequest?>
        GetLatestByUserIdAsync(
            int userId);

    Task<IEnumerable<RoleUpgradeRequest>>
        GetPendingAsync();

    Task<int> ApproveAsync(
        int requestId,
        int reviewedByUserId,
        string? remarks);

    Task<int> RejectAsync(
        int requestId,
        int reviewedByUserId,
        string? remarks);
}