namespace SimpleCRUDAPI.Ecommerce.Domain.Entities;

public class ApplicationLog
{
    public long LogId { get; set; }

    public string LogLevel { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? ExceptionMessage { get; set; }

    public string? StackTrace { get; set; }

    public string? Source { get; set; }

    public string? MethodName { get; set; }

    public string? RequestPath { get; set; }

    public int? UserId { get; set; }

    public string? IpAddress { get; set; }

    public string? MachineName { get; set; }

    public DateTime LoggedOn { get; set; }
}