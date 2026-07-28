using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Services;

public class ExceptionLogService : IExceptionLogService
{
    private readonly IExceptionLogRepository _exceptionLogRepository;

    public ExceptionLogService(IExceptionLogRepository exceptionLogRepository)
    {
        _exceptionLogRepository = exceptionLogRepository;
    }

    public async Task LogExceptionAsync(ApplicationLog log)
    {
        await _exceptionLogRepository.LogExceptionAsync(log);
    }
}