using EdenEats.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Data.Configuration
{
    public sealed class IdentityUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUser");

            builder
                .Property(p => p.UserName)
                .IsRequired(false);

            builder
                .Property(p => p.NormalizedUserName)
                .IsRequired(false);

            builder
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}
