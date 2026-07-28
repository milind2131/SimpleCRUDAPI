using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IExceptionLogService
{
    Task LogExceptionAsync(ApplicationLog log);
}