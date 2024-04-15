using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Jwt
{
    public interface IJwtService
    {
        string CreateToken(IEnumerable<Claim> claims);
        ClaimsPrincipal ValidateToken(string accessToken);
        Task<ClaimsIdentity> ValidateTokenAsync(string accessToken);
        IEnumerable<Claim> GetClaimsFromToken(string accessToken);
    }
}
