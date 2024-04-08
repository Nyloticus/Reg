using Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.MSSQL;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class DbContextServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMssqlDbContext(configuration);

            services
        .AddIdentity<User, Role>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
        }
    }
}