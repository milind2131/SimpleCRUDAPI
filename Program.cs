using Serilog;
using SimpleCRUDAPI.Ecommerce.API.Extensions;
using SimpleCRUDAPI.Ecommerce.API.Middleware;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Configurations;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Services;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/bootstrap-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();



try
{
    Log.Information("Starting SimpleCRUDAPI");

    var builder = WebApplication.CreateBuilder(args);

    // Connect ASP.NET Core ILogger<T> with Serilog
    builder.Services.AddSerilog((services, loggerConfiguration) =>
        loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

    builder.Services.Configure<EmailSettings>(
        builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddControllers();

    builder.Services.AddApplicationServices();

    builder.Services.AddJwtAuthentication(
        builder.Configuration);

    builder.Services.AddSwaggerDocumentation();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("ReactApp", policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Keep this BEFORE ExceptionMiddleware
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext =
            (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set(
                    "TraceId",
                    httpContext.TraceIdentifier);

                diagnosticContext.Set(
                    "ClientIP",
                    httpContext.Connection
                        .RemoteIpAddress?
                        .ToString());
            };
    });

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.UseCors("ReactApp");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("SimpleCRUDAPI started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "SimpleCRUDAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}