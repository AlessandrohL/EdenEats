using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Contracts.Entities
{
    public interface IUserIdentity
    {
        Guid Id { get; set; }
        string? RefreshToken { get; set; }
        DateTime? RefreshTokenExpiryTime { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }

        void SetRefreshToken(string refreshToken, DateTime expiryTime);
        void RemoveRefreshToken();
    }
}
