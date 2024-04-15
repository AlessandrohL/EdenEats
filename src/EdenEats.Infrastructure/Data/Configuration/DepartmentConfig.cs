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
    public sealed class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("DepartmentId");

            builder
                .Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .HasMany(p => p.Provinces)
                .WithOne(p => p.Department)
                .HasForeignKey(p => p.DepartmentId)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .HasData(
                    new Department { Id = 1, Name = "Lima" },
                    new Department { Id = 2, Name = "Cajamarca" },
                    new Department { Id = 3, Name = "Ayacucho" },
                    new Department { Id = 4, Name = "Cusco" }
                );
        }
    }
}
