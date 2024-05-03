using EdenEats.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdenEats.Application.DTOs;

namespace EdenEats.Application.Contracts.Auth
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(
            UserRegistrationDTO registrationRequest,
            CancellationToken cancellationToken);
        Task<AuthResponseDTO> SignInAsync(
            SignInRequestDTO signInRequest,
            CancellationToken cancellationToken);
        Task SignOutAsync(CancellationToken cancellationToken);
        Task ConfirmEmailAsync(
            EmailConfirmationDTO confirmationRequest,
            CancellationToken cancellationToken);
        Task<RefreshTokenResponseDTO> RefreshAccessTokenAsync(
            RefreshTokenDTO refreshTokenRequest,
            CancellationToken cancellationToken);
        string CreateRefreshToken();
        DateTime GenerateExpirationDateForRefreshToken();
    }
}
