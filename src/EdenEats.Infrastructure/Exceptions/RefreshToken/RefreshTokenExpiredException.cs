
using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;

namespace EdenEats.Infrastructure.Exceptions.RefreshToken
{
    public sealed class RefreshTokenExpiredException : UnauthorizedException
    {
        public RefreshTokenExpiredException(IEnumerable<string> errors) 
            : base(ErrorKeys.Auth, errors)
        { }

        public RefreshTokenExpiredException()
            : this(new string[1] { "The refresh token has expired. Please log in again to obtain a new token." })
        { }
    }
}