using SimpleCRUDAPI.Ecommerce.Application.DTOs;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IRoleUpgradeRequestService
{
    Task<RoleUpgradeOperationResultDto>
        CreateAsync(
            int userId,
            CreateRoleUpgradeRequestDto request);

    Task<RoleUpgradeRequestResponseDto?>
        GetLatestByUserIdAsync(
            int userId);

    Task<IEnumerable<RoleUpgradeRequestResponseDto>>
        GetPendingAsync();

    Task<RoleUpgradeOperationResultDto>
        ApproveAsync(
            int requestId,
            int reviewedByUserId,
            ReviewRoleUpgradeRequestDto request);

    Task<RoleUpgradeOperationResultDto>
        RejectAsync(
            int requestId,
            int reviewedByUserId,
            ReviewRoleUpgradeRequestDto request);
}