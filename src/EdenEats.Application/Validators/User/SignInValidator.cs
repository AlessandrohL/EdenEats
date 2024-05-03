using EdenEats.Application.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Validators.User
{
    public sealed class SignInValidator : AbstractValidator<SignInRequestDTO>
    {
        public SignInValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
