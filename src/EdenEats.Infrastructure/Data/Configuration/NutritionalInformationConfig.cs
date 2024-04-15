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
    public sealed class NutritionalInformationConfig : IEntityTypeConfiguration<NutritionalInformation>
    {
        public void Configure(EntityTypeBuilder<NutritionalInformation> builder)
        {
            builder.ToTable("NutritionalInformation");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("NutritionalInformationId");

            builder
                .Property(p => p.Id)
                .UseIdentityColumn();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
