using EdenEats.Domain.Constants;
using EdenEats.Infrastructure.Authentication.Claims;
using EdenEats.Infrastructure.Authentication.Config;
using EdenEats.WebApi.Common;
using EdenEats.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EdenEats.WebApi.ActionFilters
{
    public sealed class ValidateCsrfTokenFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
                throw new InvalidOperationException("The access token was not found in the request.");

            var hasHeaderCsrf = httpContext.Request.Headers.TryGetValue(CsrfConfiguration.HeaderName, out var value);
            var headerCsrfToken = value.ToString();

            if (!hasHeaderCsrf || string.IsNullOrWhiteSpace(headerCsrfToken))
            {
                var statusCode = StatusCodes.Status403Forbidden;
                httpContext.Response.StatusCode = statusCode;
                var errorResponse = new ErrorResponse(
                    HttpStatusHelper.GetTitleByStatusCode(statusCode),
                    new()
                    {
                        { ErrorKeys.Auth, new string[1] { "CSRF token is missing or empty." } }
                    },
                    statusCode);

                await httpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            var csrfClaim = claimsIdentity.FindFirst(AuthClaimNames.Csrf);
            var jwtCsrfToken = csrfClaim!.Value;

            if (!jwtCsrfToken!.Equals(headerCsrfToken))
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("CSRF is invalid!");
                return;
            }

            await next();
        }
    }
}
