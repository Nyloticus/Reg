using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Web.Builders.Interfaces;

namespace Web.Extensions
{
    public static class ServiceSetupExtension
    {
        public static void InstallConfigureInAssembly(this IServiceCollection app, IConfiguration configuration)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
              typeof(IServiceSetup).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IServiceSetup>().ToList();

            installers.ForEach(installer => installer.InstallService(app, configuration));
        }
    }
}
