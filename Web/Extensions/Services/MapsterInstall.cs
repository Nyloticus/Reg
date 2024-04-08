using Application.Auth.Register;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class MapsterInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            // config.EnableJsonMapping();
            config.Scan(new Assembly[] {
        (typeof(Startup)).Assembly,
        (typeof(RegisterCommand)).Assembly,
      });

        }
    }
}