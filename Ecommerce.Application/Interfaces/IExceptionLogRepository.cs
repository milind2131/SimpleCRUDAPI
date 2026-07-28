using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IExceptionLogRepository
{
    Task LogExceptionAsync(ApplicationLog log);
}