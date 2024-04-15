using Application.Common;
using AutoMapper;
using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Identity.Services
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<UserIdentityDTO>> CreateAsync(UserRegistrationDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var appUser = new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email,
                PhoneNumber = user.Phone
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);

            if (!result.Succeeded)
            {
                var identityErrors = result.Errors.Select(e => e.Description);
                return Result<UserIdentityDTO>.Failure(identityErrors);
            }

            var userIdentityDto = _mapper.Map<UserIdentityDTO>(appUser);

            return Result<UserIdentityDTO>.Success(userIdentityDto);
        }

        public async Task<UserIdentityDTO?> FindByIdAsync(Guid identityId)
        {
            return await _userManager
                .Users
                .Where(u => u.Id == identityId)
                .Select(u => new UserIdentityDTO(
                    u.Id,
                    u.Email,
                    u.EmailConfirmed,
                    u.PhoneNumber,
                    u.LockoutEnabled,
                    u.SecurityStamp,
                    u.RefreshToken,
                    u.RefreshTokenExpiryTime))
                .FirstOrDefaultAsync();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserIdentityDTO userIdentityDTO)
        {
            var appUser = _mapper.Map<ApplicationUser>(userIdentityDTO);
            return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<bool> IsEmailAlreadyInUseAsync(string email, CancellationToken cancellationToken)
        {
            return await _userManager.IsEmailAlreadyInUseAsync(email, cancellationToken);
        }
    }
}
