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
        public const string DefaultMessage = "The access token has not been provided or is invalid.";

        public InvalidAccessTokenException(string error) 
            : base(ErrorKeys.Auth, error)
        { }

        public InvalidAccessTokenException()
            : this(DefaultMessage)
        { }
    }
}
