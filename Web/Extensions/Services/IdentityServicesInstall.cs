using Common.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Web.Builders.Interfaces;
using Web.Globals;

namespace Web.Extensions.Services
{


    public class IdentityServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var passwordOption = configuration?.GetSection(Sections.Password)?.Get<PasswordOption>();
            var identityLockoutOption = configuration?.GetSection(Sections.IdentityLockout)?.Get<IdentityLockoutOption>();

            serviceCollection.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identityLockoutOption?.DefaultLockoutTimeSpan ?? 5);
                options.Lockout.MaxFailedAccessAttempts = identityLockoutOption?.MaxFailedAccessAttempts ?? 5;
                options.Lockout.AllowedForNewUsers = identityLockoutOption?.AllowedForNewUsers ?? true;

                options.Password.RequiredLength = passwordOption?.RequiredLength ?? 6;
                options.Password.RequireNonAlphanumeric = passwordOption?.RequireNonAlphanumeric ?? false;
                options.Password.RequireLowercase = passwordOption?.RequireLowercase ?? false;
                options.Password.RequireUppercase = passwordOption?.RequireUppercase ?? false;
                options.Password.RequireDigit = passwordOption?.RequireDigit ?? false;
            });
        }
    }
}
