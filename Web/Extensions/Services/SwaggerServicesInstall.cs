using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Web.Builders.Interfaces;

namespace Web.Extensions.Services
{
    public class SwaggerServicesInstall : IServiceSetup
    {
        public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
              .AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new OpenApiInfo()
                  {
                      Version = "v1",
                      Title = "Task-API",
                      Description = "This the description of all Apps service APIs",
                      Contact = new OpenApiContact()
                      {
                          Name = "Apps",
                          Email = string.Empty,
                      },
                      License = new OpenApiLicense()
                      {
                          Name = "Use under Apps",
                      }
                  });
                  c.AddSecurityDefinition("Bearer",
              new OpenApiSecurityScheme()
              {
                  In = ParameterLocation.Header,
                  Description = "Please enter into field the word 'Bearer' following by space and JWT",
                  Name = "Authorization",
                  Type = SecuritySchemeType.ApiKey,
                  Scheme = "Bearer"
              });
                  c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
            {
              new OpenApiSecurityScheme()
              {
                Reference = new OpenApiReference()
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
              },
              new List<string>()
            },
                });
              })
              ;
        }
    }
}