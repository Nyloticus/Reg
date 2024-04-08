using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class AppServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
              .AddScoped<IAppDbContext>(s => s.GetService<AppDbContext>()!)
              .AddScoped<IIdentityService, IdentityService>()
              .AddHttpClient()
              .AddHttpContextAccessor()
              .AddControllers();
        }
    }
}