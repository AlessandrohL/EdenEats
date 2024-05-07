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
        public const string DefaultMessage = "The refresh token has expired. Please log in again to obtain a new token.";

        public RefreshTokenNotSetException() 
            : base(ErrorKeys.Auth, DefaultMessage)
        { }

    }
}
