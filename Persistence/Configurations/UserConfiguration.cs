using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(t => t.FirstName)
                .HasMaxLength(20);
            builder.Property(t => t.MiddleName)
                .HasMaxLength(40);
            builder.Property(t => t.LastName)
                .HasMaxLength(20);

            builder.HasMany(a => a.Addresses)
                .WithOne(u => u.User)
                .HasForeignKey(uId => uId.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
