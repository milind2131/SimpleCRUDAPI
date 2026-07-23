using Serilog;
using SimpleCRUDAPI.Ecommerce.API.Extensions;
using SimpleCRUDAPI.Ecommerce.API.Middleware;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddControllers();

builder.Services.AddApplicationServices();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Logging
app.Logger.LogInformation("Application Started");
app.Logger.LogWarning("Warning Test");
app.Logger.LogError("Error Test");

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();