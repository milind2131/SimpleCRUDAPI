using System.Data;

namespace SimpleCRUDAPI.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
