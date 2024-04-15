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
    public sealed class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("OrderId");

            builder
                .Property(p => p.Status)
                .IsRequired();

            builder
                .Property(p => p.Address)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.Total)
                .HasPrecision(10, 2);

            builder
                .HasMany(p => p.Foods)
                .WithMany(p => p.Orders)
                .UsingEntity<OrderFood>();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
