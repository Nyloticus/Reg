using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserImagesConfiguration : BaseConfiguration<UserImages, string>
    {


        public override void CustomConfig(EntityTypeBuilder<UserImages> builder)
        {
            builder.HasOne(e => e.User)
                .WithMany(e => e.UserImages).HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
