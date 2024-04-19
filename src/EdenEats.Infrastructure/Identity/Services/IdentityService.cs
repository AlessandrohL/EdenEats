using Application.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Application.Exceptions.User;
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

        public async Task<Result> ConfirmEmailAsync(Guid identityId, string token)
        {
            if (identityId == Guid.Empty) throw new ArgumentException($"{nameof(identityId)} is empty Guid.");
            if (string.IsNullOrEmpty(token)) throw new ArgumentException($"{nameof(token)} is null or empty.");

            var appUser = await _userManager.FindByIdAsync(identityId.ToString());

            if (appUser == null)
            {
                throw new UserNotFoundException();
            }

            var identityResult = await _userManager.ConfirmEmailAsync(appUser, token);

            if (!identityResult.Succeeded)
            {
                var identityErrors = identityResult.Errors.Select(e => e.Description);
                return Result.Failure(identityErrors);
            }

            return Result.Success();
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
                .AsNoTracking()
                .Where(u => u.Id == identityId)
                .ProjectTo<UserIdentityDTO>(_mapper.ConfigurationProvider)
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

        public async Task<bool> IsUserExistsAsync(Guid identityId, CancellationToken cancellationToken)
        {
            return await _userManager
                .Users
                .AnyAsync(u => u.Id == identityId, cancellationToken);
        }

        public async Task<bool> IsUserEmailConfirmedAsync(Guid identityId, CancellationToken cancellationToken)
        {
            return await _userManager
                .Users
                .AnyAsync(u => u.Id == identityId && u.EmailConfirmed, cancellationToken);
        }
    }
}
