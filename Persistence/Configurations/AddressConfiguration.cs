using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class AddressConfiguration : BaseConfiguration<Address, string>
    {
        public override void CustomConfig(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(g => g.Governate)
                .WithMany(a => a.Addresses)
                .HasForeignKey(aId => aId.GovernateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.City)
                .WithMany(a => a.Addresses)
                .HasForeignKey(aId => aId.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
