using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.MSSQL
{
    public static class MsSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddMssqlDbContext(
            this IServiceCollection serviceCollection,
            IConfiguration config = null)
        {


            serviceCollection.AddDbContext<AppDbContext, MsSqlAppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                  b => b
                  .MigrationsAssembly(typeof(MsSqlAppDbContext).Assembly.FullName));
            });
            return serviceCollection;
        }
    }
}
