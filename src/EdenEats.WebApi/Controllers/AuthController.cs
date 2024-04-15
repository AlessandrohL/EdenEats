using EdenEats.Application.DTOs.Auth;
using EdenEats.Application.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EdenEats.WebApi.Extensions;
using EdenEats.WebApi.Common;

namespace EdenEats.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IValidator<UserRegistrationDTO> _registrationValidator;

        public AuthController(
            IAuthenticationService authenticationService,
            IValidator<UserRegistrationDTO> registrationValidator)
        {
            _authService = authenticationService;
            _registrationValidator = registrationValidator;
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

            await _authService.RegisterUserAsync(userRegistration, cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                SuccessResponse<string>
                    .Created("Your account has been successfully created. Please verify your email address to complete the registration process."));
        }
    }
}
