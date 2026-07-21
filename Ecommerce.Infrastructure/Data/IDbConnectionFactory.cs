using System.Data;

namespace SimpleCRUDAPI.Ecommerce.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
