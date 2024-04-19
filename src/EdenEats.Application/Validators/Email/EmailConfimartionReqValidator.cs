using EdenEats.Application.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Validators.Email
{
    public sealed class EmailConfimartionReqValidator : AbstractValidator<EmailConfirmationDTO>
    {
        private const string Base64UrlRegex = @"^([0-9a-zA-Z_\-]{4})*([0-9a-zA-Z_\-]{2})?$";

        public EmailConfimartionReqValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .Must(id => Guid.TryParse(id, out _))
                    .WithMessage("Id is not valid.");

            RuleFor(p => p.Code)
                .NotEmpty()
                .Matches(Base64UrlRegex)
                    .WithMessage("Code is not valid.");
        }

    }
}
