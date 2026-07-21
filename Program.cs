using Ecommerce.Infrastructure.Repositories;
using Serilog;
using SimpleCRUDAPI.Ecommerce.API.Middleware;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Application.Service;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using SimpleCRUDAPI.Mapping;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((context, services, loggerConfiguration) =>
//{
//    loggerConfiguration
//        .ReadFrom.Configuration(context.Configuration)
//        .ReadFrom.Services(services);
//});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register Service
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();


var app = builder.Build();

app.Logger.LogInformation("Application Started");

app.Logger.LogWarning("Warning Test");

app.Logger.LogError("Error Test");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();


//app.UseAuthentication();


app.MapControllers();

app.Run();
