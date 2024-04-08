using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
        UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IAppDbContext
    {

        #region Dbsets
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserImages> UserImages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Governate> Governates { get; set; }
        public DbSet<UserCount> UserCounts { get; set; }

        #endregion


        public AppDbContext(DbContextOptions options)
        : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(builder);
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderDisplayName).HasMaxLength(256));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(256));

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.Property(p => p.RoleId).HasMaxLength(256);
                userRole.Property(p => p.UserId).HasMaxLength(256);

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await this.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            this.Database.CommitTransaction();
        }

        public void Rollback()
        {
            this.Database.RollbackTransaction();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }
    }

}
