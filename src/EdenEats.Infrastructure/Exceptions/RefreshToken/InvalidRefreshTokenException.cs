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
        public InvalidRefreshTokenException(IEnumerable<string> errors) 
            : base(ErrorKeys.Auth, errors)
        { }

        public InvalidRefreshTokenException()
            : this(new string[1] { "The refresh token provided is invalid." })
        { }
    }
}
