using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.DTOs.Identity
{
    public record UserIdentityDTO(
        Guid Id,
        string Email,
        bool EmailConfirmed,
        string PhoneNumber,
        bool LockoutEnabled,
        string? SecurityStamp,
        string? RefreshToken,
        DateTime? RefreshTokenExpiryTime);

}
