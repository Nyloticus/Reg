using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using Web.Builders.Interfaces;

namespace Web.Extensions
{

    public static class ConfigurationSetupExtension
    {
        public static void InstallConfigureInAssembly(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
              typeof(IConfigurationSetup).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IConfigurationSetup>().ToList();

            installers.ForEach(installer => installer.SetupConfiguration(app, env));
        }
    }
}