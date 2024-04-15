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
    public sealed class FoodCommentConfig : IEntityTypeConfiguration<FoodComment>
    {
        public void Configure(EntityTypeBuilder<FoodComment> builder)
        {
            builder.ToTable("FoodComment");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("FoodCommentId");

            builder
                .HasOne(p => p.Customer)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.CustomerId);

            builder
                .HasOne(p => p.Food)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.FoodId);

            builder
                .HasIndex(fc => new { fc.FoodId, fc.CustomerId })
                .IsUnique();

            builder
                .Property(p => p.Content)
                .HasMaxLength(255);

            builder
                .Property(p => p.Rating)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
