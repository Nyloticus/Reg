using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.MSSQL
{
    public class MsSqlContextFactory : IDesignTimeDbContextFactory<MsSqlAppDbContext>
    {
        public MsSqlAppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", false)
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = config.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString, c =>
            {

                c.MigrationsAssembly(typeof(MsSqlAppDbContext).Assembly.FullName);
            });

            return new MsSqlAppDbContext(builder.Options);
        }
    }
}
