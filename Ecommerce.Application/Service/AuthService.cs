using SimpleCRUDAPI.Ecommerce.Application.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Response;
using SimpleCRUDAPI.Ecommerce.Application.Exceptions;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Domain.Entities;
using SimpleCRUDAPI.Model;

namespace SimpleCRUDAPI.Ecommerce.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request)
    {
        var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new UserAlreadyExistsException();
        //throw new Exception("User already exists.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            MobileNumber = request.MobileNumber,
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };

        var userId = await _authRepository.RegisterUserAsync(user);

        return new RegisterResponseDto
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            MobileNumber = user.MobileNumber,
            Message = "Registration Successful."
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _authRepository.GetUserByEmailAsync(request.Email);

        if (user == null)
            throw new InvalidCredentialsException();       // Fluent validation with customize response structure.
        //throw new Exception("Invalid Email or Password.");

        var isValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!isValid)
            throw new Exception("Invalid Email or Password.");

        var jwt = _jwtTokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = jwt.Token,
            Expiration = jwt.Expiration
        };
    }
}