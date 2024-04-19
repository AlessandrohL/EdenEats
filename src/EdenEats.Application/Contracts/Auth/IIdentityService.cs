using Application.Common;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Auth
{
    public interface IIdentityService
    {
        Task<bool> IsEmailAlreadyInUseAsync(string email, CancellationToken cancellationToken);
        Task<bool> IsUserExistsAsync(Guid identityId, CancellationToken cancellationToken);
        Task<bool> IsUserEmailConfirmedAsync(Guid identityId, CancellationToken cancellationToken);
        Task<Result<UserIdentityDTO>> CreateAsync(UserRegistrationDTO user);
        Task<UserIdentityDTO?> FindByIdAsync(Guid identityId);
        Task<Result> ConfirmEmailAsync(Guid identityId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(UserIdentityDTO identityDTO);
    }
}
