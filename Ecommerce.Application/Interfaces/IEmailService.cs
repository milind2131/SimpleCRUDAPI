namespace SimpleCRUDAPI.Ecommerce.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string firstName, string otp);
    }
}