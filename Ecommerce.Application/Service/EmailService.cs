using Microsoft.Extensions.Options;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using SimpleCRUDAPI.Ecommerce.Infrastructure.Configurations;
using System.Net;
using System.Net.Mail;

namespace SimpleCRUDAPI.Ecommerce.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendOtpEmailAsync(string email, string firstName, string otp)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = "Email Verification OTP",
                IsBodyHtml = true,
                Body = $@"
                        <html>
                        <body style='font-family:Arial'>
                            <h2>Hello {firstName},</h2>

                            <p>Thank you for registering with ECommerce Web API.</p>

                            <p>Your One Time Password (OTP) is:</p>

                            <h1 style='color:blue'>{otp}</h1>

                            <p>This OTP is valid for <b>5 minutes</b>.</p>

                            <p>If you did not request this OTP, please ignore this email.</p>

                            <br/>

                            <p>Regards,</p>
                            <p><b>ECommerce Team</b></p>

                        </body>
                        </html>"
            };

            mail.To.Add(email);

            using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port);

            smtp.Credentials = new NetworkCredential(
                _emailSettings.FromEmail,
                _emailSettings.AppPassword);

            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);
        }
    }
}