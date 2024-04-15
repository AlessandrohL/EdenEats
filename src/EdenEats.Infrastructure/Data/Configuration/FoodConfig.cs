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
    public sealed class FoodConfig : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.ToTable("Food");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("FoodId");

            builder
                .Property(p => p.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .HasIndex(p => p.Slug)
                .IsUnique();

            builder
                .Property(p => p.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(p => p.QuantityAvailable)
                .IsRequired();

            builder
                .Property(p => p.Price)
                .HasPrecision(10, 2)
                .IsRequired();

            builder
                .Property(p => p.ImageUrl)
                .IsRequired();

            builder
                .HasOne(p => p.NutritionalInformation)
                .WithOne(p => p.Food)
                .HasForeignKey<NutritionalInformation>(p => p.FoodId);

            builder
                .HasMany(p => p.Instructions)
                .WithOne(p => p.Food)
                .HasForeignKey(p => p.FoodId);

            builder
                .HasMany(p => p.Ingredients)
                .WithMany(p => p.Foods)
                .UsingEntity<FoodIngredient>();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
