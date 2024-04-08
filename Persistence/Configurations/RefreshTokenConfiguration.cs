using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(a => a.Token);
            builder.Property(a => a.Token)
              .ValueGeneratedOnAdd();

            builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);

        }
    }
}