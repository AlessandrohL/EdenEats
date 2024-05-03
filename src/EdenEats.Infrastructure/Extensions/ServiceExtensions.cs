using EdenEats.Infrastructure.Data.Interceptors;
using EdenEats.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EdenEats.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using EdenEats.Domain.Contracts.Repositories;
using EdenEats.Infrastructure.Data.Repositories;
using EdenEats.Application.Contracts.Auth;
using EdenEats.Application.Contracts.Email;
using EdenEats.Infrastructure.Email;
using EdenEats.Application.Contracts.Jwt;
using EdenEats.Infrastructure.Jwt;
using EdenEats.Infrastructure.Authentication.Services;
using EdenEats.Infrastructure.Authentication.Config;
using EdenEats.Infrastructure.Client;
using EdenEats.Infrastructure.Storage;

namespace EdenEats.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddAuthenticationWithJWT(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var cookieSettings = configuration.GetSection("CookieConfiguration");
            var cookieName = cookieSettings["CookieName"];

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = cookieName;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSettings["ValidIssuer"],
                        ValidAudience = jwtSettings["ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            context.Token = context.Request.Cookies[cookieName!];
                            return Task.CompletedTask;
                        }
                    };
                });
                
        }

        public static void AddDbContext(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            string connectionStr = configuration.GetConnectionString("DevConnection")
               ?? throw new Exception($"{nameof(connectionStr)} Connection string is null.");

            services.AddDbContext<ApplicationDbContext>(
                (sp, options) => options
             .UseSqlServer(connectionStr)
             .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>()));
        }

        public static void AddIdentityCore(this IServiceCollection services)
        {
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
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IAntiforgeryService, AntiforgeryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IJwtService, JwtService>();
        }

        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            var cloudinaryConfig = configuration
                .GetSection("CloudinaryConfiguration")
                .Get<CloudinaryConfiguration>();

            var clientConfig = configuration
                .GetSection("ClientConfiguration")
                .Get<ClientConfiguration>();

            var cookieConfig = configuration
                .GetSection("CookieConfiguration")
                .Get<CookieConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddSingleton(cloudinaryConfig);
            services.AddSingleton(clientConfig);
            services.AddSingleton(cookieConfig);
        }
    
    }
}
