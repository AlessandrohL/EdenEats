using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Contracts.Utilities;
using EdenEats.Domain.Contracts.Repositories;
using EdenEats.Domain.Contracts.UnitOfWorks;
using EdenEats.Infrastructure.Client;
using EdenEats.Infrastructure.Data;
using EdenEats.Infrastructure.Data.Interceptors;
using EdenEats.Infrastructure.Data.Repositories;
using EdenEats.Infrastructure.Data.UnitOfWorks;
using EdenEats.Infrastructure.Email;
using EdenEats.Infrastructure.Identity;
using EdenEats.Infrastructure.Identity.Services;
using EdenEats.Infrastructure.Jwt;
using EdenEats.Infrastructure.Storage;
using EdenEats.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DevConnection")
                ?? throw new ArgumentNullException("Connection string is null.");

            services.AddAutoMapper(
                Assembly.GetAssembly(typeof(Application.DependencyInjection)),
                Assembly.GetAssembly(typeof(DependencyInjection)));

            services.AddSingleton<SoftDeleteInterceptor>();

            services.AddDbContext<ApplicationDbContext>(
                (sp, options) => options
                    .UseSqlServer(connection)
                    .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>()));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.AllowedForNewUsers = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDataProtection();

            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            var cloudinaryConfig = configuration
                .GetSection("CloudinaryConfiguration")
                .Get<CloudinaryConfiguration>();

            var clientConfig = configuration
                .GetSection("ClientConfiguration")
                .Get<ClientConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddSingleton(cloudinaryConfig);
            services.AddSingleton(clientConfig);

            services.AddSingleton<JwtConfiguration>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUrlUtility, UrlUtility>();
            return services;
        }
    }
}
