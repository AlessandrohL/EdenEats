using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Exceptions.RefreshToken
{
    public sealed class RefreshTokenNotSetException : UnprocessableException
    {
        public RefreshTokenNotSetException(IEnumerable<string> errors) 
            : base(ErrorKeys.Auth, errors)
        { }

        public RefreshTokenNotSetException()
            : this(new string[1] { "User does not have a refresh token set." })
        { }
    }
}
