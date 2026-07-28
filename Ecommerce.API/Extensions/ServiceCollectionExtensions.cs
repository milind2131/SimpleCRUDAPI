using Ecommerce.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Application.Service;
using SimpleCRUDAPI.Ecommerce.Application.Services;
using SimpleCRUDAPI.Ecommerce.Application.Validators;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Data;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Security;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Services;
using SimpleCRUDAPI.Mapping;
namespace SimpleCRUDAPI.Ecommerce.API.Extensions;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {

        //services.Configure<ApiBehaviorOptions>(options =>
        //{
        //    options.SuppressModelStateInvalidFilter = true;
        //});

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IEmailService, EmailService>();


        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
        services.AddScoped<IExceptionLogService, ExceptionLogService>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
}