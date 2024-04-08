using Boxed.AspNetCore;
using Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Builders.Interfaces;
using Web.Globals;

namespace Web.Extensions.Services
{


    public class OptionsServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
              .ConfigureAndValidateSingleton<JwtOption>(configuration.GetSection(nameof(Sections.JwtOption)))
              .ConfigureAndValidateSingleton<AppInfoOption>(configuration.GetSection(nameof(Sections.AppInfoOption)))
              ;
        }
    }
}