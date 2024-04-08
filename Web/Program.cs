using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Serilog;
using System.Threading.Tasks;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AppDbContext>();

                var concreteContext = context;
                if (concreteContext != null)
                {
                    await concreteContext.Database.MigrateAsync();
                    await AppDbInitializer.Initialize(concreteContext, scope, "");
                }

            }
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)                
              .UseSerilog((context, conf) =>
              {
                  var env = context.HostingEnvironment;
                  var config = new ConfigurationBuilder()              
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
              .AddEnvironmentVariables()
              .Build();
                  conf.ReadFrom.Configuration(config)
              .Enrich.FromLogContext()
              .Enrich.WithProperty("ApplicationName", "Task-Service")
              ;
              })
              .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}