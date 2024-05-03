using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Auth
{
    public interface IUserContext
    {
        Guid UserId { get; init; }
        bool IsAuthenticated { get; init; }
        string? GetAccessToken();
        void SetAccessTokenCookie(string accessToken);
        void RemoveAccessTokenCookie();
    }
}
