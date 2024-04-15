using EdenEats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Data.Configuration
{
    public sealed class ProvinceConfig : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Province");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("ProvinceId");

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasMany(p => p.Districts)
                .WithOne(p => p.Province)
                .HasForeignKey(p => p.ProvinceId)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .HasData(
                    new Province { Id = 1, Name = "Lima", DepartmentId = 1 },
                    new Province { Id = 2, Name = "San Ignacio", DepartmentId = 2 },
                    new Province { Id = 3, Name = "La Mar", DepartmentId = 3 },
                    new Province { Id = 4, Name = "La Convención", DepartmentId = 4 }
                );
        }
    }
}
