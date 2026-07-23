using FluentValidation;
using SimpleCRUDAPI.Ecommerce.Application.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.DTOs.Request;

namespace SimpleCRUDAPI.Ecommerce.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid Email Address.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile Number is required.")
            .Matches(@"^[6-9]\d{9}$")
            .WithMessage("Please enter a valid 10-digit Mobile Number.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("This message is coming from FluentValidation..")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}