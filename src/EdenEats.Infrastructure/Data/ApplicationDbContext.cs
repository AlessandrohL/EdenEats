using EdenEats.Domain.Contracts.Entities;
using EdenEats.Domain.Entities;
using EdenEats.Infrastructure.Data.Configuration;
using EdenEats.Infrastructure.Data.Extensions;
using EdenEats.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Province> Provinces { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Food> Foods { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;
        public DbSet<FoodIngredient> FoodIngredients { get; set; } = null!;
        public DbSet<Allergen> Allergens { get; set; } = null!;
        public DbSet<IngredientAllergen> IngredientAllergens { get; set; } = null!;
        public DbSet<Instruction> Instructions { get; set; } = null!;
        public DbSet<NutritionalInformation> NutritionalInformations { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderFood> OrderFoods { get; set; } = null!;
        public DbSet<FoodComment> Comments { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateDateTimeModifiedEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CustomerConfig());
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new DepartmentConfig());
            builder.ApplyConfiguration(new ProvinceConfig());
            builder.ApplyConfiguration(new DistrictConfig());
            builder.ApplyConfiguration(new CategoryConfig());
            builder.ApplyConfiguration(new FoodConfig());
            builder.ApplyConfiguration(new IngredientConfig());
            builder.ApplyConfiguration(new NutritionalInformationConfig());
            builder.ApplyConfiguration(new OrderConfig());
            builder.ApplyConfiguration(new OrderFoodConfig());
            builder.ApplyConfiguration(new FoodCommentConfig());
            builder.ApplyConfiguration(new AllergenConfig());
            builder.ApplyConfiguration(new InstructionConfig());

            builder.ApplyConfiguration(new IdentityUserConfig());
            builder.ApplyConfiguration(new IdentityRoleConfig());
            builder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRole"); });
            builder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaim"); });
            builder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogin"); });
            builder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaim"); });

            ApplySoftDeleteQueryFilter(builder);
        }

        private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(type.ClrType))
                {
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
                }
            }
        }

        private void UpdateDateTimeModifiedEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is BaseEntity trackableEntity)
                {
                    trackableEntity.ModifiedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
