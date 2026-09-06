using FluentValidation;
using SimpleCRUDAPI.Ecommerce.Application.Exceptions;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Constants;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using System.Diagnostics;
using System.Security.Claims;

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

    public async Task InvokeAsync(
        HttpContext context,
        IExceptionLogService exceptionLogService)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "Validation failed for {RequestMethod} {RequestPath}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 400,
                Message = "Validation failed.",
                Errors = ex.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                })
            });
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(
                ex,
                "Business exception occurred for {RequestMethod} {RequestPath}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 400,
                Message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(
                ex,
                "Unauthorized request for {RequestMethod} {RequestPath}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 401,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            await HandleUnhandledExceptionAsync(
                context,
                ex,
                exceptionLogService);
        }
    }

    private async Task HandleUnhandledExceptionAsync(
        HttpContext context,
        Exception exception,
        IExceptionLogService exceptionLogService)
    {
        var traceId = context.TraceIdentifier;

        var origin = GetExceptionOrigin(exception);

        var userIdClaim =
            context.User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        int? userId = null;

        if (int.TryParse(userIdClaim, out var parsedUserId))
        {
            userId = parsedUserId;
        }

        // This will be written to Serilog/file.
        _logger.LogError(
            exception,
            "Unhandled exception. TraceId: {TraceId}. " +
            "Request: {RequestMethod} {RequestPath}. " +
            "Origin: {FileName}:{LineNumber} - {MethodName}",
            traceId,
            context.Request.Method,
            context.Request.Path,
            origin.FileName,
            origin.LineNumber,
            origin.MethodName);

        var log = new ApplicationLog
        {
            LogLevel = LogLevels.Error,

            Message = exception.Message,

            ExceptionType =
                exception.GetType().FullName,

            // ex.ToString() contains exception type,
            // message, inner exceptions and complete details.
            ExceptionMessage =
                exception.ToString(),

            StackTrace =
                exception.StackTrace,

            Source =
                exception.Source,

            FileName =
                origin.FileName,

            LineNumber =
                origin.LineNumber,

            MethodName =
                origin.MethodName,

            RequestMethod =
                context.Request.Method,

            RequestPath =
                context.Request.Path,

            TraceId =
                traceId,

            UserId =
                userId,

            IpAddress =
                context.Connection.RemoteIpAddress?
                    .MapToIPv4()
                    .ToString(),

            MachineName =
                Environment.MachineName
        };

        try
        {
            await exceptionLogService
                .LogExceptionAsync(log);
        }
        catch (Exception loggingException)
        {
            // VERY IMPORTANT:
            // Never allow DB logging failure to hide
            // the original exception.

            _logger.LogError(
                loggingException,
                "Failed to save exception to database. " +
                "Original TraceId: {TraceId}",
                traceId);
        }

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(new
        {
            StatusCode = 500,

            Message =
                "An unexpected error occurred.",

            TraceId = traceId
        });
    }

    private static ExceptionOrigin GetExceptionOrigin(
        Exception exception)
    {
        var stackTrace =
            new StackTrace(exception, true);

        var frames = stackTrace.GetFrames();

        if (frames == null ||
            frames.Length == 0)
        {
            return new ExceptionOrigin();
        }

        var applicationFrame =
            frames.FirstOrDefault(frame =>
            {
                var method =
                    frame.GetMethod();

                var namespaceName =
                    method?
                        .DeclaringType?
                        .Namespace;

                if (namespaceName == null)
                    return false;

                return
                    namespaceName.StartsWith(
                        "SimpleCRUDAPI",
                        StringComparison.OrdinalIgnoreCase)

                    || namespaceName.StartsWith(
                        "Ecommerce",
                        StringComparison.OrdinalIgnoreCase)

                    || namespaceName.StartsWith(
                        "ECommerce",
                        StringComparison.OrdinalIgnoreCase);
            });

        applicationFrame ??=
            frames.FirstOrDefault();

        var methodInfo =
            applicationFrame?.GetMethod();

        var fullFilePath =
            applicationFrame?.GetFileName();

        var lineNumber =
            applicationFrame?
                .GetFileLineNumber() ?? 0;

        return new ExceptionOrigin
        {
            FileName =
                string.IsNullOrWhiteSpace(fullFilePath)
                    ? null
                    : Path.GetFileName(fullFilePath),

            LineNumber =
                lineNumber > 0
                    ? lineNumber
                    : null,

            MethodName =
                methodInfo == null
                    ? null
                    : $"{methodInfo.DeclaringType?.FullName}.{methodInfo.Name}"
        };
    }

    private class ExceptionOrigin
    {
        public string? FileName { get; set; }

        public int? LineNumber { get; set; }

        public string? MethodName { get; set; }
    }
}