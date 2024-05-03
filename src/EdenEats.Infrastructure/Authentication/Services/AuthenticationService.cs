using AutoMapper;
using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Contracts.Jwt;
using EdenEats.Application.Contracts.Utilities;
using EdenEats.Application.DTOs;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.Email;
using EdenEats.Application.Exceptions.Email;
using EdenEats.Application.Exceptions.User;
using EdenEats.Domain.Contracts.Repositories;
using EdenEats.Domain.Contracts.UnitOfWorks;
using EdenEats.Domain.Entities;
using EdenEats.Infrastructure.Authentication.Claims;
using EdenEats.Infrastructure.Exceptions.Jwt;
using EdenEats.Infrastructure.Exceptions.RefreshToken;
using EdenEats.Infrastructure.Extensions;
using EdenEats.Infrastructure.Identity;
using EdenEats.Infrastructure.Identity.Extensions;
using EdenEats.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Authentication.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUrlUtility _urlUtility;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IAntiforgeryService _forgeryService;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IUrlUtility urlUtility,
            IEmailService emailService,
            IJwtService jwtService,
            JwtConfiguration jwtConfig,
            IAntiforgeryService forgeryService,
            IUserContext userContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _urlUtility = urlUtility;
            _emailService = emailService;
            _jwtService = jwtService;
            _jwtConfig = jwtConfig;
            _forgeryService = forgeryService;
            _userContext = userContext;
        }

        public async Task RegisterAsync(UserRegistrationDTO registrationRequest, CancellationToken cancellationToken)
        {
            var isEmailInUse = await _userManager.IsEmailAlreadyInUseAsync(
                email: registrationRequest.Email,
                cancellationToken);

            if (isEmailInUse)
            {
                throw new UserAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(registrationRequest);
            var appUser = new ApplicationUser
            {
                PhoneNumber = registrationRequest.Phone,
                Email = registrationRequest.Email
            };

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var identityResult = await _userManager.CreateAsync(appUser, registrationRequest.Password);

                if (!identityResult.Succeeded)
                {
                    var errors = identityResult.Errors.Select(e => e.Description);
                    throw new UserRegistrationFailedException(errors);
                }

                customer.AssignIdentity(appUser.Id);
                _customerRepository.Create(customer);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var encodedToken = _urlUtility.EncodeUrlToBase64(confirmationToken);
            var confirmationInfo = new EmailConfirmationInfo(appUser.Id, appUser.Email, encodedToken, customer.FirstName);

            await _emailService.SendEmailConfirmationAsync(confirmationInfo);
        }

        public async Task<AuthResponseDTO> SignInAsync(SignInRequestDTO signInRequest, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByEmailAsync(signInRequest.Email);

            if (appUser == null)
            {
                throw new UserNotFoundException();
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(appUser, signInRequest.Password);

            if (!isPasswordCorrect)
            {
                throw new UserInvalidCredentialsException();
            }

            if (!appUser.EmailConfirmed)
            {
                throw new UnconfirmedUserEmailException();
            }

            var refreshToken = CreateRefreshToken();
            var refreshTokenExpiration = GenerateExpirationDateForRefreshToken();

            appUser.SetRefreshToken(refreshToken, refreshTokenExpiration);
            await _userManager.UpdateAsync(appUser);

            var csrfToken = _forgeryService.GenerateToken();

            var claims = GenerateClaims(appUser.Id.ToString(), csrfToken!);
            var accessToken = _jwtService.CreateToken(claims);

            _userContext.SetAccessTokenCookie(accessToken);

            return new AuthResponseDTO(refreshToken, csrfToken!);
        }

        public async Task ConfirmEmailAsync(EmailConfirmationDTO confirmationRequest, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByIdAsync(confirmationRequest.Id);

            if (appUser == null)
            {
                throw new UserNotFoundException();
            }

            if (appUser.EmailConfirmed)
            {
                throw new UserAlreadyConfirmedException();
            }

            var decodedToken = _urlUtility.DecodeBase64Url(confirmationRequest.Code);
            var confirmationResult = await _userManager.ConfirmEmailAsync(appUser, decodedToken);

            if (!confirmationResult.Succeeded)
            {
                var errors = confirmationResult.Errors.Select(e => e.Description);
                throw new EmailConfirmationFailedException(errors);
            }
        }

        public string CreateRefreshToken()
        {
            var bytes = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        public DateTime GenerateExpirationDateForRefreshToken()
            => DateTime.UtcNow.AddDays(_jwtConfig.GetRefreshTokenValidityDays());

        public static IEnumerable<Claim> GenerateClaims(string userId, string csrfToken)
        {
            return new HashSet<Claim>()
            {
                new (JwtRegisteredClaimNames.Sub, userId),
                new (AuthClaimNames.Csrf, csrfToken)
            };
        }

        public async Task SignOutAsync(CancellationToken cancellationToken)
        {
            if (!_userContext.IsAuthenticated)
            {
                throw new Exception("User not authenticated.");
            }

            if (_userContext.UserId == Guid.Empty)
            {
                throw new Exception("UserId not found.");
            }

            var appUser = await _userManager.FindByIdAsync(_userContext.UserId.ToString());
            
            if (appUser == null)
            {
                throw new UserNotFoundException();
            }

            appUser.RemoveRefreshToken();
            await _userManager.UpdateAsync(appUser);

            _userContext.RemoveAccessTokenCookie();
        }

        public async Task<RefreshTokenResponseDTO> RefreshAccessTokenAsync(RefreshTokenDTO refreshTokenRequest, CancellationToken cancellationToken)
        {
            var accessToken = _userContext.GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidAccessTokenException();
            }

            var claimsIdentity = await _jwtService.ValidateTokenAsync(accessToken!);
            var userId = claimsIdentity.GetUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (string.IsNullOrEmpty(user.RefreshToken))
            {
                throw new RefreshTokenNotSetException();
            }

            if (user.RefreshToken != refreshTokenRequest.RefreshToken)
            {
                throw new InvalidRefreshTokenException();
            }

            if (DateTime.UtcNow > user.RefreshTokenExpiryTime)
            {
                throw new RefreshTokenExpiredException();
            }

            var csrfToken = _forgeryService.GenerateToken();
            var claims = GenerateClaims(user.Id.ToString(), csrfToken!);

            var renewedAccessToken = _jwtService.CreateToken(claims);

             _userContext.SetAccessTokenCookie(renewedAccessToken);

            return new RefreshTokenResponseDTO(csrfToken!);
        }


    }
}
