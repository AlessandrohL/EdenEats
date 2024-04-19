using AutoMapper;
using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Contracts.Utilities;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Application.Exceptions.Email;
using EdenEats.Application.Exceptions.User;
using EdenEats.Domain.Contracts.Entities;
using EdenEats.Domain.Contracts.Repositories;
using EdenEats.Domain.Contracts.UnitOfWorks;
using EdenEats.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IUrlUtility _urlUtility;

        public AuthenticationService(
           ICustomerRepository customerRepository,
           IIdentityService identityService,
           IUnitOfWork unitOfWork,
           IMapper mapper,
           IEmailService emailService,
           IUrlUtility urlUtility)
        {
            _customerRepository = customerRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _urlUtility = urlUtility;
        }

        public async Task RegisterUserAsync(UserRegistrationDTO registrationRequest, CancellationToken cancellationToken)
        {
            var emailInUse = await _identityService.IsEmailAlreadyInUseAsync(
                email: registrationRequest.Email,
                cancellationToken);

            if (emailInUse)
            {
                throw new UserAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(registrationRequest);
            UserIdentityDTO userIdentity;

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var result = await _identityService.CreateAsync(registrationRequest);

                if (result.IsFailure)
                {
                    throw new UserRegistrationFailedException(result.Errors!);
                }

                userIdentity = result.Value!;
                customer.AssignIdentity(userIdentity.Id);

                _customerRepository.Create(customer);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            var confirmationToken = await _identityService.GenerateEmailConfirmationTokenAsync(userIdentity);
            var encodedToken = _urlUtility.EncodeUrlToBase64(confirmationToken);

            await _emailService.SendEmailConfirmationAsync(
                userIdentity: userIdentity,
                confirmationToken: encodedToken,
                names: registrationRequest.FirstName);
        }

        public Task SignInAsync(SignInRequestDTO signInRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmUserEmailAsync(EmailConfirmationDTO confirmationRequest, CancellationToken cancellationToken)
        {
            var identityId = Guid.Parse(confirmationRequest.Id);

            if (!await _identityService.IsUserExistsAsync(identityId, cancellationToken))
            {
                throw new UserNotFoundException();
            }

            if (await _identityService.IsUserEmailConfirmedAsync(identityId, cancellationToken))
            {
                throw new UserAlreadyConfirmedException();
            }

            var decodedToken = _urlUtility.DecodeBase64Url(confirmationRequest.Code);

            var confirmationResult = await _identityService.ConfirmEmailAsync(identityId, decodedToken);

            if (confirmationResult.IsFailure)
            {
                throw new EmailConfirmationFailedException(confirmationResult.Errors!);
            }
        }

        public string CreateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public DateTime GenerateExpirationDateToRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(string accessToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAccessTokenAsync(string accessToken, RefreshTokenDTO refreshTokenRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRefreshTokenAsync(IUserIdentity user)
        {
            throw new NotImplementedException();
        }
    }
}
