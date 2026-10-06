namespace SimpleCRUDAPI.Ecommerce.Application.DTOs;

public class RoleUpgradeRequestResponseDto
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string RequestedRole { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime RequestedOn { get; set; }

    public DateTime? ReviewedOn { get; set; }

    public string? ReviewRemarks { get; set; }
}