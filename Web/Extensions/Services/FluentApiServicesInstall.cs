using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Web.Builders.Interfaces;


namespace Web.Extensions.Services
{


    public class FluentApiServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; })
              .AddFluentValidation(config =>
              {
                  config.DisableDataAnnotationsValidation = false;
                  config.RegisterValidatorsFromAssemblies(new List<Assembly>() {
          typeof(Application.Auth.Register.RegisterCommand.Validator).Assembly
                });
              });
        }
    }
}
