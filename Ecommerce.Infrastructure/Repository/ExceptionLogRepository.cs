using Dapper;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Constants;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories;

public class ExceptionLogRepository : IExceptionLogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ExceptionLogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task LogExceptionAsync(ApplicationLog log)
    {
        using IDbConnection connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@LogLevel", log.LogLevel);
        parameters.Add("@Message", log.Message);
        parameters.Add("@ExceptionMessage", log.ExceptionMessage);
        parameters.Add("@StackTrace", log.StackTrace);
        parameters.Add("@Source", log.Source);
        parameters.Add("@MethodName", log.MethodName);
        parameters.Add("@RequestPath", log.RequestPath);
        parameters.Add("@UserId", log.UserId);
        parameters.Add("@IpAddress", log.IpAddress);
        parameters.Add("@MachineName", log.MachineName);

        await connection.ExecuteAsync(
            StoredProcedures.InsertExceptionLog,
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}