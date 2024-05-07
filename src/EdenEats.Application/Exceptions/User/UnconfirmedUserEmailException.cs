using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Exceptions.User
{
    public sealed class UnconfirmedUserEmailException : UnauthorizedException
    {
        public const string DefaultMessage = "The user's email address has not yet been confirmed.";

        public UnconfirmedUserEmailException()
            : base(ErrorKeys.User, DefaultMessage)
        { }
    }
}
