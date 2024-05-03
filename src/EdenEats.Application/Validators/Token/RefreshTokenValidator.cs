using EdenEats.Application.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Validators.Token
{
    public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenDTO>
    {
        public RefreshTokenValidator()
        {
            RuleFor(p => p.RefreshToken)
                .NotEmpty();
        }
    }
}
