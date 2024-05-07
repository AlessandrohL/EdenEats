
using EdenEats.Domain.Constants;
using EdenEats.Domain.Exceptions.BaseExceptions;

namespace EdenEats.Infrastructure.Exceptions.RefreshToken
{
    public sealed class RefreshTokenExpiredException : UnauthorizedException
    {
        public const string DefaultMessage = "The refresh token has expired. Please log in again to obtain a new token.";

        public RefreshTokenExpiredException() 
            : base(ErrorKeys.Auth, DefaultMessage)
        { }

    }
}