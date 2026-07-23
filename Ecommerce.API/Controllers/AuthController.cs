using Microsoft.AspNetCore.Mvc;
using SimpleCRUDAPI.Ecommerce.Application.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
}