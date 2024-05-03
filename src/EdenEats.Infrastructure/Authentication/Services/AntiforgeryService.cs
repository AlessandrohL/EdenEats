using EdenEats.Application.Contracts.Auth;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Authentication.Services
{
    public sealed class AntiforgeryService : IAntiforgeryService
    {
        private readonly IAntiforgery _antiforgery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AntiforgeryService(
            IAntiforgery antiforgery,
            IHttpContextAccessor httpContextAccessor)
        {
            _antiforgery = antiforgery;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken()
        {
            return _antiforgery.GetTokens(_httpContextAccessor.HttpContext!).RequestToken!;
        }
    }
}
