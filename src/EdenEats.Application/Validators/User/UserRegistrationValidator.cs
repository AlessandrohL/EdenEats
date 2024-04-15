using EdenEats.Application.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Validators.User
{
    public sealed class UserRegistrationValidator : AbstractValidator<UserRegistrationDTO>
    {
        private const string NamesRegex = @"^[^\s\d][a-zA-Z\s]*$";
        private const string PhoneRegex = @"^9\d{8}$";
        private const string PasswordRegex = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";

        public UserRegistrationValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty()
                .Matches(NamesRegex)
                    .WithMessage("Name must not start with a space or a number, and can only contain letters and spaces.")
                .MaximumLength(60)
                .MinimumLength(2);

            When(p => string.IsNullOrWhiteSpace(p.LastName), () =>
            {
                RuleFor(p => p.LastName)
                    .Matches(NamesRegex)
                        .WithMessage("Last name must not start with a space or a number, and can only contain letters and spaces.")
                    .MaximumLength(70);
            });

            RuleFor(p => p.Phone)
                .NotEmpty()
                .Length(9)
                .Matches(PhoneRegex)
                    .WithMessage("Invalid phone number.");

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(PasswordRegex)
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (#?!@$%^&*-).");
        }
    }
}
