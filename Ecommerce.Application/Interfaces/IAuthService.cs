using ECommerce.API.DTOs.Auth;
using ECommerce.Application.DTOs.Auth;
using SimpleCRUDAPI.Ecommerce.Application.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Response;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<ChangePasswordResponseDto> ChangePasswordAsync(int userId,ChangePasswordRequestDto request);
    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
    Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
    Task<VerifyOtpResponseDto> VerifyOtpAsync(VerifyOtpRequestDto request);
    Task<ResendOtpResponseDto> ResendOtpAsync(ResendOtpRequestDto request);

}