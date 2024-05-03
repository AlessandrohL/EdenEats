using EdenEats.Application.Contracts.Email;
using EdenEats.Application.Contracts.Utilities;
using EdenEats.Domain.Contracts.UnitOfWorks;
using EdenEats.Infrastructure.Authentication.Config;
using EdenEats.Infrastructure.Data.Interceptors;
using EdenEats.Infrastructure.Data.UnitOfWorks;
using EdenEats.Infrastructure.Email;
using EdenEats.Infrastructure.Extensions;
using EdenEats.Infrastructure.Jwt;
using EdenEats.Infrastructure.Utilities;
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
            services.AddDbContext(configuration);

            services.AddAutoMapper(
                Assembly.GetAssembly(typeof(Application.DependencyInjection)),
                Assembly.GetAssembly(typeof(DependencyInjection)));

            services.AddSingleton<SoftDeleteInterceptor>();

            services.AddAntiforgery(options => options.HeaderName = CsrfConfiguration.HeaderName);

            services.AddAuthenticationWithJWT(configuration);

            services.AddIdentityCore();

            services.AddDataProtection();

            services.AddConfigurations(configuration);
            services.AddRepositories();
            services.AddServices();

            services.AddSingleton<JwtConfiguration>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUrlUtility, UrlUtility>();

            return services;
        }
    }
}
