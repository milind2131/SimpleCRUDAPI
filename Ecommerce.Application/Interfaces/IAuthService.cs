using SimpleCRUDAPI.Ecommerce.Application.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Response;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}