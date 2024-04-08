using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class GovernateConfiguration : BaseConfiguration<Governate, string>
    {
        public override void CustomConfig(EntityTypeBuilder<Governate> builder)
        {
        }
    }
}
