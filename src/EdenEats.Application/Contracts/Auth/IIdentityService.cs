using Application.Common;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Domain.Contracts.Entities;
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
        Task<Result<UserIdentityDTO>> CreateAsync(UserRegistrationDTO user);
        Task<UserIdentityDTO?> FindByIdAsync(Guid identityId);
        Task<string> GenerateEmailConfirmationTokenAsync(UserIdentityDTO identityDTO);
    }
}
