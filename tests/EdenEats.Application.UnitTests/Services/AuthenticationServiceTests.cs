using Application.Common;
using AutoFixture;
using AutoFixture.NUnit3;
using AutoMapper;
using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Contracts.Utilities;
using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.DTOs.Identity;
using EdenEats.Application.Exceptions.User;
using EdenEats.Application.Services;
using EdenEats.Domain.Contracts.Repositories;
using EdenEats.Domain.Contracts.UnitOfWorks;
using EdenEats.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.UnitTests.Services
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IIdentityService> _mockIdentityService; 
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUrlUtility> _mockUrlUtility;
        private readonly IFixture _fixture;
        private readonly AuthenticationService _authService;
        private Mock<IDbTransaction> _mockDbTransaction;

        public AuthenticationServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmailService = new Mock<IEmailService>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _mockUrlUtility = new Mock<IUrlUtility>();
            _authService = new AuthenticationService(
                _mockCustomerRepository.Object,
                _mockIdentityService.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockEmailService.Object,
                _mockUrlUtility.Object);
            _mockDbTransaction = new Mock<IDbTransaction>();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void Configure()
        {
            _mockUnitOfWork
                .Setup(p => p.SaveChangesAsync(default))
                .ReturnsAsync(It.IsAny<int>());
        }

        [Test]
        [AutoData]
        public async Task RegisterUserAsync_EmailNotInUse_SuccesfulRegisteredUser(
            UserRegistrationDTO userRegistrationDto)
        {
            _mockIdentityService
                .Setup(p => p.IsEmailAlreadyInUseAsync(userRegistrationDto.Email, default))
                .ReturnsAsync(false)
                .Verifiable();

            var customerMapped = _fixture.Create<Customer>();

            _mockMapper
                .Setup(p => p.Map<Customer>(userRegistrationDto))
                .Returns(customerMapped)
                .Verifiable();

            _mockDbTransaction
                .Setup(p => p.Commit())
                .Verifiable();

            _mockUnitOfWork
                .Setup(p => p.BeginTransaction())
                .Returns(_mockDbTransaction.Object)
                .Verifiable();

            var userIdentityDto = _fixture.Build<UserIdentityDTO>()
                .With(p => p.Email, userRegistrationDto.Email)
                .With(p => p.PhoneNumber, userRegistrationDto.Phone)
                .Create();

            _mockIdentityService
                .Setup(p => p.CreateAsync(userRegistrationDto))
                .ReturnsAsync(Result<UserIdentityDTO>.Success(userIdentityDto))
                .Verifiable();

            string confirmationToken = _fixture.Create<string>();

            _mockIdentityService
                .Setup(p => p.GenerateEmailConfirmationTokenAsync(userIdentityDto))
                .ReturnsAsync(confirmationToken)
                .Verifiable();

            _mockEmailService
                .Setup(p => p.SendEmailConfirmationAsync(
                    userIdentityDto,
                    confirmationToken,
                    userRegistrationDto.FirstName))
                .Verifiable();

            await _authService.RegisterUserAsync(userRegistrationDto, default);

            _mockIdentityService.Verify(p => p.IsEmailAlreadyInUseAsync(userRegistrationDto.Email, default), Times.Once);
            _mockMapper.Verify(p => p.Map<Customer>(userRegistrationDto), Times.Once);
            _mockUnitOfWork.Verify(p => p.BeginTransaction(), Times.Once);
            _mockIdentityService.Verify(p => p.CreateAsync(userRegistrationDto), Times.Once);
            _mockIdentityService.Verify(p => p.GenerateEmailConfirmationTokenAsync(userIdentityDto), Times.Once);
            _mockEmailService.Verify(
                p => p.SendEmailConfirmationAsync(
                    userIdentityDto,
                    confirmationToken,
                    userRegistrationDto.FirstName),
                Times.Once);

            Assert.That(customerMapped.IdentityId, Is.Not.EqualTo(Guid.Empty));

            _mockDbTransaction.Verify(p => p.Commit(), Times.Once);
        }

        [Test]
        [AutoData]
        public void RegisterUserAsync_EmailAlreadyExists_ThrowUserAlreadyExistsException(
            UserRegistrationDTO userRegistrationDto)
        { 
            _mockIdentityService
                .Setup(p => p.IsEmailAlreadyInUseAsync(userRegistrationDto.Email, default))
                .ReturnsAsync(true)
                .Verifiable();

            Assert.ThrowsAsync<UserAlreadyExistsException>(
                async () => await _authService.RegisterUserAsync(userRegistrationDto, default));

            _mockIdentityService.Verify(
                p => p.IsEmailAlreadyInUseAsync(userRegistrationDto.Email, default),
                Times.Once);
        }

        [Test]
        [AutoData]
        public void RegisterUserAsync_RegistrationFails_ThrowsUserRegistrationFailedException(
            UserRegistrationDTO userRegistration)
        {
            _mockIdentityService
                .Setup(p => p.IsEmailAlreadyInUseAsync(userRegistration.Email, default))
                .ReturnsAsync(false)
                .Verifiable();

            var customer = _fixture.Create<Customer>();

            _mockMapper
                .Setup(p => p.Map<Customer>(userRegistration))
                .Returns(customer)
                .Verifiable();

            _mockDbTransaction
                .Setup(p => p.Rollback())
                .Verifiable();

            _mockUnitOfWork
                .Setup(p => p.BeginTransaction())
                .Returns(_mockDbTransaction.Object)
                .Verifiable();

            var fakeErrors = _fixture.CreateMany<string>(3);
            _mockIdentityService
                .Setup(p => p.CreateAsync(userRegistration))
                .ReturnsAsync(Result<UserIdentityDTO>.Failure(fakeErrors));

            Assert.ThrowsAsync<UserRegistrationFailedException>(
                async () => await _authService.RegisterUserAsync(userRegistration, default));

            _mockDbTransaction.Verify(p => p.Rollback(), Times.Once);
        }

        [Test]
        public void ConfirmUserEmailAsync_WhenEmailConfirmationIsSuccessful_ShouldNotThrowAnyException()
        {
            var identityId = _fixture.Create<Guid>();
            var confirmationRequest = _fixture.Build<EmailConfirmationDTO>()
                .With(p => p.Id, identityId.ToString())
                .Create();

            _mockIdentityService
                .Setup(p => p.IsUserExistsAsync(identityId, default))
                .ReturnsAsync(true);

            _mockIdentityService
                .Setup(p => p.IsUserEmailConfirmedAsync(identityId, default))
                .ReturnsAsync(false);

            var fakeCode = _fixture.Create<string>();

            _mockUrlUtility
                .Setup(p => p.DecodeBase64Url(confirmationRequest.Code))
                .Returns(fakeCode);

            _mockIdentityService
                .Setup(p => p.ConfirmEmailAsync(identityId, fakeCode))
                .ReturnsAsync(Result.Success());

            Assert.DoesNotThrowAsync(
                async () => await _authService.ConfirmUserEmailAsync(confirmationRequest, default));
        }

        [Test]
        public void ConfirmUserEmailAsync_WhenUserDoesNotExist_ShouldThrowUserNotFoundException()
        {
            var identityId = _fixture.Create<Guid>();
            var confirmationRequest = _fixture.Build<EmailConfirmationDTO>()
                .With(p => p.Id, identityId.ToString())
                .Create();

            _mockIdentityService
                .Setup(p => p.IsUserExistsAsync(identityId, default))
                .ReturnsAsync(false);

            Assert.ThrowsAsync<UserNotFoundException>(
                async () => await _authService.ConfirmUserEmailAsync(confirmationRequest, default));
        }

        [Test]
        public void ConfirmUserEmailAsync_WhenUserEmailIsAlreadyConfirmed_ShouldThrowUserAlreadyConfirmedException()
        {
            var identityId = _fixture.Create<Guid>();
            var confirmationRequest = _fixture.Build<EmailConfirmationDTO>()
                .With(p => p.Id, identityId.ToString())
                .Create();

            _mockIdentityService
                .Setup(p => p.IsUserExistsAsync(identityId, default))
                .ReturnsAsync(true);

            _mockIdentityService
                .Setup(p => p.IsUserEmailConfirmedAsync(identityId, default))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<UserAlreadyConfirmedException>(
                async () => await _authService.ConfirmUserEmailAsync(confirmationRequest, default));
        }
    }
}
