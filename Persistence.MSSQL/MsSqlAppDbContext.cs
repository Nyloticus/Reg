using Microsoft.EntityFrameworkCore;

namespace Persistence.MSSQL
{
    public class MsSqlAppDbContext : AppDbContext
    {
        public MsSqlAppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
