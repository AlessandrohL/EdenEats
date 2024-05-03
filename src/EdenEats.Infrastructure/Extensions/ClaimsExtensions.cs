using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            string userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
        }

        public static Guid GetUserId(this ClaimsIdentity identity)
        {
            var claimIdentifier = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (claimIdentifier == null) 
                return Guid.Empty;

            return Guid.TryParse(claimIdentifier.Value, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
        }
    }
}
