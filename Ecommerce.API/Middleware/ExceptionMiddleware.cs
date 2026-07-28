using FluentValidation;
using SimpleCRUDAPI.Ecommerce.Application.Exceptions;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Constants;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace SimpleCRUDAPI.Ecommerce.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IExceptionLogService _exceptionLogService;

    // Getting error as middleware runs per request so using this.
    private readonly IServiceScopeFactory _scopeFactory;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _logger = logger;
        _scopeFactory = scopeFactory;
       // _exceptionLogService = exceptionLogService;
    }


    public async Task Invoke(HttpContext context, IExceptionLogService exceptionLogService)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed.");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 400,
                Message = "Validation Failed.",
                Errors = ex.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                })
            });
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 400,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.InnerException?.Message);
            var endpoint = context.GetEndpoint();
            var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var StackTrace = ex.StackTrace?
                .Split(Environment.NewLine)
                .FirstOrDefault(x => x.Contains("SimpleCRUDAPI"));


            var log = new ApplicationLog
            {
                LogLevel = LogLevels.Error,
                Message = ex.Message,
                ExceptionMessage = ex.InnerException?.Message,
                StackTrace = StackTrace,
                Source = ex.Source,
                UserId= userIdClaim != null ? Convert.ToInt32(userIdClaim.Value): null,
                MethodName = endpoint?.DisplayName,
                RequestPath = context.Request.Path,
                IpAddress = context.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
                MachineName = Environment.MachineName
            };


            /* Getting error as middleware runs per request so using this below extra two lines instead we can directly
            inject the scoped service into the Invoke method instead of the constructor..*/
            //using var scope = _scopeFactory.CreateScope();
            //var exceptionLogService =scope.ServiceProvider.GetRequiredService<IExceptionLogService>();
            await exceptionLogService.LogExceptionAsync(log);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 500,
                Message = "An unexpected error occurred. Please try again later."
            });
        }
    }
}