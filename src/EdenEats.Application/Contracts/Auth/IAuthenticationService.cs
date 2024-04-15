using EdenEats.Domain.Contracts.Entities;
using EdenEats.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Auth
{
    public interface IAuthenticationService
    {
        Task RegisterUserAsync(
            UserRegistrationDTO registrationRequest,
            CancellationToken cancellationToken);
        Task SignInAsync(
            SignInRequestDTO signInRequest,
            CancellationToken cancellationToken);
        Task LogoutAsync(string accessToken, CancellationToken cancellationToken);
        Task ConfirmUserEmailAsync(
            EmailConfirmationDto confirmationRequest,
            CancellationToken cancellationToken);
        Task RefreshAccessTokenAsync(
            string accessToken,
            RefreshTokenDTO refreshTokenRequest,
            CancellationToken cancellationToken);
        string CreateRefreshToken();
        DateTime GenerateExpirationDateToRefreshToken();
        //Task RemoveRefreshTokenAsync(TUser user);
    }
}
