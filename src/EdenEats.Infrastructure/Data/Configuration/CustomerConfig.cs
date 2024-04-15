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
    public sealed class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("CustomerId");

            builder
                .Property(p => p.FirstName)
                .HasMaxLength(60)
                .IsRequired();

            builder
                .Property(p => p.LastName)
                .HasMaxLength(70);

            builder
                .HasIndex(p => p.IdentityId)
                .IsUnique();

            builder
                .Property(p => p.IdentityId)
                .HasMaxLength(36)
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(p => p.Phone)
                .HasMaxLength(9)
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .HasMany(p => p.Addresses)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .IsRequired();

            builder
                .HasMany(p => p.Orders)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
