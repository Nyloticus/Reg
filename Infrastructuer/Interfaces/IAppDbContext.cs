using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;
        DbSet<User> Users { get; set; }
        DbSet<UserImages> UserImages { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Governate> Governates { get; set; }
        DbSet<UserCount> UserCounts { get; set; }
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> CreateTransaction();

        void Commit();
        void Rollback();
    }
}