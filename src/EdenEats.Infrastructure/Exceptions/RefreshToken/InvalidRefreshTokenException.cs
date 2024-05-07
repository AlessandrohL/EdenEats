using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Exceptions.RefreshToken
{
    public sealed class InvalidRefreshTokenException : UnauthorizedException
    {
        public const string DefaultMessage = "The refresh token provided is invalid.";

        public InvalidRefreshTokenException() 
            : base(ErrorKeys.Auth, DefaultMessage)
        { }

    }
}
