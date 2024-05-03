using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EdenEats.WebApi.Extensions;
using EdenEats.WebApi.Common;
using EdenEats.Application.DTOs;

namespace EdenEats.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IValidator<UserRegistrationDTO> _registrationValidator;
        private readonly IValidator<EmailConfirmationDTO> _emailConfirmationValidator;
        private readonly IValidator<SignInRequestDTO> _signInValidator;
        private readonly IValidator<RefreshTokenDTO> _refreshTokenValidator;

        public AuthController(
            IAuthenticationService authenticationService,
            IValidator<UserRegistrationDTO> registrationValidator,
            IValidator<EmailConfirmationDTO> emailConfirmationValidator,
            IValidator<SignInRequestDTO> signInValidator,
            IValidator<RefreshTokenDTO> refreshTokenValidator)
        {
            _authService = authenticationService;
            _registrationValidator = registrationValidator;
            _emailConfirmationValidator = emailConfirmationValidator;
            _signInValidator = signInValidator;
            _refreshTokenValidator = refreshTokenValidator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(
            [FromBody] UserRegistrationDTO userRegistration,
            CancellationToken cancellationToken)
        {
            var result = _registrationValidator.Validate(userRegistration);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            await _authService.RegisterAsync(userRegistration, cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                SuccessResponse<string>
                    .Created("Your account has been successfully created. Please verify your email address to complete the registration process."));
        }

        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(
            [FromBody] EmailConfirmationDTO emailConfirmation,
            CancellationToken cancellationToken)
        {
            var result = _emailConfirmationValidator.Validate(emailConfirmation);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            await _authService.ConfirmEmailAsync(emailConfirmation, cancellationToken);

            return Ok(SuccessResponse.NoContent());
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInUser(
            [FromBody] SignInRequestDTO signInRequest,
            CancellationToken cancellationToken)
        {
            var result = _signInValidator.Validate(signInRequest);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            var authResponse = await _authService.SignInAsync(signInRequest, cancellationToken);

            return Ok(SuccessResponse<AuthResponseDTO>.Ok(authResponse));
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken(
            [FromBody] RefreshTokenDTO refreshTokenRequest,
            CancellationToken cancellationToken)
        {
            var result = _refreshTokenValidator.Validate(refreshTokenRequest);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

            var refreshResponse = await _authService.RefreshAccessTokenAsync(refreshTokenRequest, cancellationToken);

            return Ok(SuccessResponse<RefreshTokenResponseDTO>.Ok(refreshResponse));
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
        {
            await _authService.SignOutAsync(cancellationToken);

            return Ok(SuccessResponse.NoContent());
        }


        [HttpGet("only-auth")]
        public IActionResult OnlyAuthenticated()
        {
            return Ok("Nice");
        }

    }
}
