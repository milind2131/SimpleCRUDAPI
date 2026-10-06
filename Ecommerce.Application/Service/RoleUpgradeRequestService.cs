using SimpleCRUDAPI.Ecommerce.Application.DTOs;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Services;

public class RoleUpgradeRequestService
    : IRoleUpgradeRequestService
{
    private readonly IRoleUpgradeRequestRepository
        _roleUpgradeRequestRepository;

    public RoleUpgradeRequestService(
        IRoleUpgradeRequestRepository
            roleUpgradeRequestRepository)
    {
        _roleUpgradeRequestRepository =
            roleUpgradeRequestRepository;
    }

    public async Task<RoleUpgradeOperationResultDto>
        CreateAsync(
            int userId,
            CreateRoleUpgradeRequestDto request)
    {
        var requestedRole =
            request.RequestedRole?.Trim();

        if (string.IsNullOrWhiteSpace(
                requestedRole))
        {
            return Failure(
                "Please select Seller or Admin.");
        }

        if (requestedRole.Equals(
                "Seller",
                StringComparison.OrdinalIgnoreCase))
        {
            requestedRole = "Seller";
        }
        else if (requestedRole.Equals(
                     "Admin",
                     StringComparison.OrdinalIgnoreCase))
        {
            requestedRole = "Admin";
        }
        else
        {
            return Failure(
                "Only Seller or Admin access can be requested.");
        }

        var latestRequest =
            await _roleUpgradeRequestRepository
                .GetLatestByUserIdAsync(
                    userId);

        if (latestRequest != null &&
            latestRequest.Status.Equals(
                "Pending",
                StringComparison.OrdinalIgnoreCase))
        {
            return Failure(
                $"You already have a pending {latestRequest.RequestedRole} request.");
        }

        var requestId =
            await _roleUpgradeRequestRepository
                .CreateAsync(
                    userId,
                    requestedRole);

        if (requestId == -1)
        {
            return Failure(
                "You already have a pending role request.");
        }

        if (requestId <= 0)
        {
            return Failure(
                "Unable to create role request.");
        }

        return new RoleUpgradeOperationResultDto
        {
            Success = true,
            Message =
                $"{requestedRole} access request submitted successfully.",
            RequestId = requestId
        };
    }

    public async Task<RoleUpgradeRequestResponseDto?>
        GetLatestByUserIdAsync(
            int userId)
    {
        var roleRequest =
            await _roleUpgradeRequestRepository
                .GetLatestByUserIdAsync(
                    userId);

        if (roleRequest == null)
        {
            return null;
        }

        return MapToResponse(
            roleRequest);
    }

    public async Task<
        IEnumerable<RoleUpgradeRequestResponseDto>>
        GetPendingAsync()
    {
        var requests =
            await _roleUpgradeRequestRepository
                .GetPendingAsync();

        return requests.Select(
            MapToResponse);
    }

    public async Task<RoleUpgradeOperationResultDto>
        ApproveAsync(
            int requestId,
            int reviewedByUserId,
            ReviewRoleUpgradeRequestDto request)
    {
        var result =
            await _roleUpgradeRequestRepository
                .ApproveAsync(
                    requestId,
                    reviewedByUserId,
                    request.Remarks?.Trim());

        if (result <= 0)
        {
            return Failure(
                "Request was not found or is no longer pending.");
        }

        return new RoleUpgradeOperationResultDto
        {
            Success = true,
            Message =
                "Role request approved successfully."
        };
    }

    public async Task<RoleUpgradeOperationResultDto>
        RejectAsync(
            int requestId,
            int reviewedByUserId,
            ReviewRoleUpgradeRequestDto request)
    {
        var result =
            await _roleUpgradeRequestRepository
                .RejectAsync(
                    requestId,
                    reviewedByUserId,
                    request.Remarks?.Trim());

        if (result <= 0)
        {
            return Failure(
                "Request was not found or is no longer pending.");
        }

        return new RoleUpgradeOperationResultDto
        {
            Success = true,
            Message =
                "Role request rejected successfully."
        };
    }

    private static
        RoleUpgradeRequestResponseDto MapToResponse(
            RoleUpgradeRequest request)
    {
        return new RoleUpgradeRequestResponseDto
        {
            RequestId = request.RequestId,
            UserId = request.UserId,
            Email = request.Email,
            RequestedRole = request.RequestedRole,
            Status = request.Status,
            RequestedOn = request.RequestedOn,
            ReviewedOn = request.ReviewedOn,
            ReviewRemarks = request.ReviewRemarks
        };
    }

    private static
        RoleUpgradeOperationResultDto Failure(
            string message)
    {
        return new RoleUpgradeOperationResultDto
        {
            Success = false,
            Message = message
        };
    }
}