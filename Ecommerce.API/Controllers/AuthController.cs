using ECommerce.API.DTOs.Auth;
using ECommerce.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDAPI.Ecommerce.Application.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.ECommerce.Application.DTOs;
using System.Security.Claims;

namespace SimpleCRUDAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
    VerifyOtpRequestDto request)
    {
        var response = await _authService.VerifyOtpAsync(request);

        return Ok(response);
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(
    ResendOtpRequestDto request)
    {
        var response = await _authService.ResendOtpAsync(request);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var response = await _authService.RegisterUserAsync(request);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request)
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        ChangePasswordResponseDto response = await _authService.ChangePasswordAsync(userId, request);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto request)
    {
        var response = await _authService.ForgotPasswordAsync(request);

        return Ok(response);
    }

   

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
    {
        var response = await _authService.ResetPasswordAsync(request);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var response = await _authService.RefreshTokenAsync(request);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
    {
        var response = await _authService.LogoutAsync(request);

        return Ok(response);
    }

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutFromAllDevices()
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var response = await _authService.LogoutFromAllDevicesAsync(userId);

        return Ok(response);
    }

}