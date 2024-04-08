using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CityConfiguration : BaseConfiguration<City, string>
    {
        public override void CustomConfig(EntityTypeBuilder<City> builder)
        {

        }
    }
}
