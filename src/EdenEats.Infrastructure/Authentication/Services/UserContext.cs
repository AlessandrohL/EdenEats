using EdenEats.Application.Contracts.Auth;
using EdenEats.Infrastructure.Authentication.Config;
using EdenEats.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Authentication.Services
{
    public sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CookieConfiguration _cookieConfig;
        public Guid UserId { get; init; }
        public bool IsAuthenticated { get; init; }

        public UserContext(
            IHttpContextAccessor httpContextAccessor,
            CookieConfiguration cookieConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _cookieConfig = cookieConfig;

            UserId = GetContext().User.GetUserId();
            IsAuthenticated = GetContext().User.Identity?.IsAuthenticated ?? false;
        }

        public string? GetAccessToken()
        {
            return GetContext()
                .Request
                .Cookies[key: _cookieConfig.CookieName!];
        }

        public void SetAccessTokenCookie(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentException("The access token cannot be null or empty.", nameof(accessToken));
            }

            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddDays(_cookieConfig.CookieLifetimeDays),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            };

            GetContext().Response.Cookies.Append(
                key: _cookieConfig.CookieName!,
                value: accessToken,
                options: cookieOptions);
        }

        public void RemoveAccessTokenCookie()
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            GetContext().Response.Cookies.Delete(
               key: _cookieConfig.CookieName!,
               options: cookieOptions);
        }

        private HttpContext GetContext()
            => _httpContextAccessor.HttpContext ?? 
            throw new ApplicationException("User context is unavailable");

    }
}
