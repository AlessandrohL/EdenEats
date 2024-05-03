using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Exceptions.Jwt
{
    public sealed class InvalidAccessTokenException : UnauthorizedException
    {
        public InvalidAccessTokenException(IEnumerable<string> errors) 
            : base(ErrorKeys.Auth, errors)
        { 
        }

        public InvalidAccessTokenException()
            : this(new string[1] { "The access token has not been provided or is invalid." })
        { }
    }
}
