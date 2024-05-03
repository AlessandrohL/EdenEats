using EdenEats.Application.Contracts.Jwt;
using EdenEats.Infrastructure.Exceptions.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Jwt
{
    public sealed class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfig;

        public JwtService(JwtConfiguration jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }


        public string CreateToken(IEnumerable<Claim> claims)
        {
            var signInCredentials = _jwtConfig.GetSigningCredentials();
            var tokenOptions = GenerateTokenOptions(signInCredentials, claims);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return accessToken;
        }

        private JwtSecurityToken GenerateTokenOptions(
            SigningCredentials signingCredentials,
            IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: _jwtConfig.GetIssuer(),
                audience: _jwtConfig.GetAudience(),
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.GetTokenValidityMinutes()),
                signingCredentials: signingCredentials);
        }

        public IEnumerable<Claim> GetClaimsFromToken(string accessToken)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal ValidateToken(string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimsIdentity> ValidateTokenAsync(string accessToken)
        {
            var validationParams = GetValidationParameters(false);
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationResult = await tokenHandler.ValidateTokenAsync(accessToken, validationParams);

            if (!validationResult.IsValid)
            {
                throw new InvalidAccessTokenException();
            }

            return validationResult.ClaimsIdentity;
        }

        private TokenValidationParameters GetValidationParameters(bool validateLifetime = true)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtConfig.GetSymmetricSecurityKey(),
                ValidateLifetime = validateLifetime,
                ValidIssuer = _jwtConfig.GetIssuer(),
                ValidAudience = _jwtConfig.GetAudience()
            };
        }
    }
}
