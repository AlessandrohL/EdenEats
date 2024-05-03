using EdenEats.Domain.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Identity
{
    public sealed class ApplicationUser : IdentityUser<Guid>, IUserIdentity
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public void SetRefreshToken(string refreshToken, DateTime expiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiryTime;
        }

        public void RemoveRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
        }
    }
}
