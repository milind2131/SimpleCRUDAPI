using FluentValidation;
using SimpleCRUDAPI.Ecommerce.Application.Exceptions;
using System.Text.Json;

namespace SimpleCRUDAPI.Ecommerce.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
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
            _logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 500,
                Message = "An unexpected error occurred. Please try again later."
            });
        }
    }
}