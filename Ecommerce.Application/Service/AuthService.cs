using AutoMapper;
using ECommerce.API.DTOs.Auth;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Enums;
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
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public AuthService(
     IAuthRepository authRepository,
     IPasswordHasher passwordHasher,
     IJwtTokenService jwtTokenService,
     IMapper mapper,
     IEmailService emailService)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request)
    {
        var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new UserAlreadyExistsException();

        var pendingUser = await _authRepository.GetPendingUserByEmailAsync(request.Email);

        if (pendingUser is not null)
            throw new Exception("Email verification is pending. Please verify the OTP or request a new one.");

        User user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            MobileNumber = request.MobileNumber,
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };

        string otp = Random.Shared.Next(100000, 999999).ToString();

        string otpHash = _passwordHasher.HashPassword(otp);

        DateTime otpExpiry = DateTime.UtcNow.AddMinutes(5);

        await _authRepository.InsertPendingUserAsync(user,otpHash, otpExpiry);

        await _emailService.SendOtpEmailAsync(
            user.Email,
            user.FirstName,
            otp);

        return new RegisterResponseDto
        {
            Email = user.Email,
            Message = "OTP has been sent successfully to your registered email."
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

    public async Task<ChangePasswordResponseDto> ChangePasswordAsync(int userId,ChangePasswordRequestDto request)
    {
        var user = await _authRepository.GetUserByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found.");

        if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            throw new Exception("Current password is incorrect.");

        if (request.NewPassword != request.ConfirmPassword)
            throw new Exception("New Password and Confirm Password do not match.");

        if (request.CurrentPassword == request.NewPassword)
            throw new Exception("New Password must be different from Current Password.");

        string passwordHash = _passwordHasher.HashPassword(request.NewPassword);

        bool result = await _authRepository.ChangePasswordAsync(userId, passwordHash);

        return new ChangePasswordResponseDto
        {
            IsSuccess = result,
            Message = result
                ? "Password changed successfully."
                : "Password could not be changed."
        };
    }

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        var user = await _authRepository.GetUserByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("User does not exist.");

        var passwordResetRequest = await _authRepository.GetPasswordResetRequestAsync(user.UserId);

        string otp = Random.Shared.Next(100000, 999999).ToString();

        string otpHash = _passwordHasher.HashPassword(otp);

        DateTime otpExpiry = DateTime.UtcNow.AddMinutes(5);

        if (passwordResetRequest == null)
        {
            await _authRepository.InsertPasswordResetRequestAsync(
                user.UserId,
                otpHash,
                otpExpiry);
        }
        else
        {
            await _authRepository.UpdatePasswordResetOtpAsync(
                passwordResetRequest.PasswordResetRequestId,
                otpHash,
                otpExpiry);
        }

        await _emailService.SendOtpEmailAsync(
            user.Email,
            user.FirstName,
            otp);

        return new ForgotPasswordResponseDto
        {
            Email = user.Email,
            Message = "OTP has been sent successfully."
        };
    }

    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        var user = await _authRepository.GetUserByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("User does not exist.");

        var passwordResetRequest = await _authRepository.GetPasswordResetRequestAsync(user.UserId);

        if (passwordResetRequest == null)
            throw new Exception("Password reset request not found.");

        if (!passwordResetRequest.IsVerified)
            throw new Exception("OTP verification pending.");

        string passwordHash = _passwordHasher.HashPassword(request.NewPassword);

        await _authRepository.UpdatePasswordAsync(
            user.UserId,
            passwordHash);

        await _authRepository.DeletePasswordResetRequestAsync(
            passwordResetRequest.PasswordResetRequestId);

        return new ResetPasswordResponseDto
        {
            Message = "Password reset successfully."
        };
    }

    public async Task<VerifyOtpResponseDto> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        switch (request.Purpose)
        {
            case OtpPurpose.EmailVerification:
                {
                    var pendingUser = await _authRepository.GetPendingUserByEmailAsync(request.Email);

                    if (pendingUser == null)
                        throw new Exception("Registration request not found.");

                    if (pendingUser.OTPExpiry < DateTime.UtcNow)
                        throw new Exception("OTP has expired.");

                    bool isOtpValid = _passwordHasher.VerifyPassword(
                        request.OTP,
                        pendingUser.OTPHash);

                    if (!isOtpValid)
                        throw new Exception("Invalid OTP.");

                    User newUser = new User
                    {
                        FirstName = pendingUser.FirstName,
                        LastName = pendingUser.LastName,
                        Email = pendingUser.Email,
                        MobileNumber = pendingUser.MobileNumber,
                        PasswordHash = pendingUser.PasswordHash
                    };

                    await _authRepository.RegisterUserAsync(newUser);

                    await _authRepository.DeletePendingUserAsync(
                        pendingUser.PendingUserId);

                    break;
                }

            case OtpPurpose.ForgotPassword:
                {
                    var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);

                    if (existingUser == null)
                        throw new Exception("User does not exist.");

                    var passwordResetRequest =
                        await _authRepository.GetPasswordResetRequestAsync(existingUser.UserId);

                    if (passwordResetRequest == null)
                        throw new Exception("Password reset request not found.");

                    if (passwordResetRequest.OTPExpiry < DateTime.UtcNow)
                        throw new Exception("OTP has expired.");

                    bool isValid = _passwordHasher.VerifyPassword(
                        request.OTP,
                        passwordResetRequest.OTPHash);

                    if (!isValid)
                        throw new Exception("Invalid OTP.");

                    await _authRepository.VerifyPasswordResetRequestAsync(
                        passwordResetRequest.PasswordResetRequestId);

                    break;
                }

            default:
                throw new Exception("Invalid OTP purpose.");
        }

        return new VerifyOtpResponseDto
        {
            IsSuccess = true,
            Message = "OTP verified successfully."
        };
    }

    public async Task<ResendOtpResponseDto> ResendOtpAsync(ResendOtpRequestDto request)
    {
        switch (request.Purpose)
        {
            case OtpPurpose.EmailVerification:

                var pendingUser =
                    await _authRepository.GetPendingUserByEmailAsync(request.Email);

                if (pendingUser == null)
                    throw new Exception("No pending registration found.");

                string registrationOtp =
                    Random.Shared.Next(100000, 999999).ToString();

                string registrationOtpHash =
                    _passwordHasher.HashPassword(registrationOtp);

                DateTime registrationExpiry =
                    DateTime.UtcNow.AddMinutes(5);

                await _authRepository.UpdateRegistrationOtpAsync(
                    request.Email,
                    registrationOtpHash,
                    registrationExpiry);

                await _emailService.SendOtpEmailAsync(
                    pendingUser.Email,
                    pendingUser.FirstName,
                    registrationOtp);

                break;

            case OtpPurpose.ForgotPassword:

                var user =
                    await _authRepository.GetUserByEmailAsync(request.Email);

                if (user == null)
                    throw new Exception("User does not exist.");

                var passwordResetRequest =
                    await _authRepository.GetPasswordResetRequestAsync(user.UserId);

                if (passwordResetRequest == null)
                    throw new Exception("Password reset request not found.");

                string forgotOtp =
                    Random.Shared.Next(100000, 999999).ToString();

                string forgotOtpHash =
                    _passwordHasher.HashPassword(forgotOtp);

                DateTime forgotExpiry =
                    DateTime.UtcNow.AddMinutes(5);

                await _authRepository.UpdatePasswordResetOtpAsync(
                    passwordResetRequest.PasswordResetRequestId,
                    forgotOtpHash,
                    forgotExpiry);

                await _emailService.SendOtpEmailAsync(
                    user.Email,
                    user.FirstName,
                    forgotOtp);

                break;

            default:
                throw new Exception("Invalid OTP purpose.");
        }

        return new ResendOtpResponseDto
        {
            Email = request.Email,
            Message = "OTP has been sent successfully."
        };
    }
}