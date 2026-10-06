using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDAPI.Ecommerce.Application.DTOs;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;

namespace SimpleCRUDAPI.Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleUpgradeRequestController
    : ControllerBase
{
    private readonly IRoleUpgradeRequestService
        _roleUpgradeRequestService;

    public RoleUpgradeRequestController(
        IRoleUpgradeRequestService
            roleUpgradeRequestService)
    {
        _roleUpgradeRequestService =
            roleUpgradeRequestService;
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRoleUpgradeRequestDto request)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _roleUpgradeRequestService
                .CreateAsync(
                    userId,
                    request);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return Ok(result);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var userId =
            GetCurrentUserId();

        var request =
            await _roleUpgradeRequestService
                .GetLatestByUserIdAsync(
                    userId);

        if (request == null)
        {
            return Ok(null);
        }

        return Ok(request);
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var requests =
            await _roleUpgradeRequestService
                .GetPendingAsync();

        return Ok(requests);
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{requestId:int}/approve")]
    public async Task<IActionResult> Approve(
        int requestId,
        ReviewRoleUpgradeRequestDto request)
    {
        var superAdminUserId =
            GetCurrentUserId();

        var result =
            await _roleUpgradeRequestService
                .ApproveAsync(
                    requestId,
                    superAdminUserId,
                    request);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return Ok(result);
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{requestId:int}/reject")]
    public async Task<IActionResult> Reject(
        int requestId,
        ReviewRoleUpgradeRequestDto request)
    {
        var superAdminUserId =
            GetCurrentUserId();

        var result =
            await _roleUpgradeRequestService
                .RejectAsync(
                    requestId,
                    superAdminUserId,
                    request);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return Ok(result);
    }

    private int GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
            ?.Value;

        if (!int.TryParse(
                userIdValue,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "User identifier is missing from token.");
        }

        return userId;
    }
}