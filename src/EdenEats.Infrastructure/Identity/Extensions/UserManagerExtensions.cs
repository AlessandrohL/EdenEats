using EdenEats.Domain.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Identity.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> IsEmailAlreadyInUseAsync<TUser>(
            this UserManager<TUser> userManager,
            string email,
            CancellationToken cancellationToken) where TUser : class, IUserIdentity
        {
            return await userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}
