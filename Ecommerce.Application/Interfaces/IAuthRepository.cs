using SimpleCRUDAPI.Ecommerce.Domain.Entities;

namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces;

public interface IAuthRepository
{
    Task<int> RegisterUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);

    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> ChangePasswordAsync(int userId,string passwordHash);

    Task<int> InsertPendingUserAsync(User user,string otpHash,DateTime otpExpiry);
    Task<PendingUser?> GetPendingUserByEmailAsync(string email);

    Task DeletePendingUserAsync(int pendingUserId);

    Task<int> UpdateRegistrationOtpAsync(string email,string otpHash,DateTime otpExpiry);

    Task<int> InsertPasswordResetRequestAsync(int userId,string otpHash,DateTime otpExpiry);

    Task<PasswordResetRequest?> GetPasswordResetRequestAsync( int userId);

    Task VerifyPasswordResetRequestAsync(int passwordResetRequestId);

    Task DeletePasswordResetRequestAsync( int passwordResetRequestId);

    Task<int> UpdatePasswordResetOtpAsync(int passwordResetRequestId,string otpHash,DateTime otpExpiry);

    Task UpdatePasswordAsync( int userId, string passwordHash);
}

