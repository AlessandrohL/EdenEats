using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Jwt
{
    public sealed class JwtConfiguration
    {
        private readonly IConfiguration _config;

        public JwtConfiguration(IConfiguration configuration)
            => _config = configuration;

        public string? GetIssuer() => _config["JwtSettings:ValidIssuer"];

        public string? GetAudience() => _config["JwtSettings:ValidAudience"];

        public SigningCredentials GetSigningCredentials() =>
            new (GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

        public string? GetSecurityKey() => _config["JwtSettings:Secret"];

        public byte[] GetSymmetricSecurityKeyAsBytes() => Encoding.UTF8.GetBytes(GetSecurityKey()!);

        public SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(GetSymmetricSecurityKeyAsBytes());

        public double GetTokenValidityMinutes()
            => Convert.ToDouble(_config["JwtSettings:TokenLifetimeMinutes"]);

        public double GetRefreshTokenValidityDays()
            => Convert.ToDouble(_config["JwtSettings:RefreshTokenLifetimeDays"]);

        public string? GetCookieName() => _config["JwtSettings:CookieName"];
        public double GetCookieExpirationDays()
            => Convert.ToDouble(_config["JwtSettings:CookieLifetimeDays"]);
    }
}
