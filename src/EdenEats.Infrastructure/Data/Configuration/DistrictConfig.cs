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
    public sealed class DistrictConfig : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("District");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("DistrictId");

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .HasData(
                    new District { Id = 1, Name = "San Isidro", ProvinceId = 1 },
                    new District { Id = 2, Name = "Huarango", ProvinceId = 2 },
                    new District { Id = 3, Name = "Santa Rosa", ProvinceId = 3 },
                    new District { Id = 4, Name = "Huayopata", ProvinceId = 4 }
                );
        }
    }
}
