using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserCountConfiguration : BaseConfiguration<UserCount, string>
    {
        public override void CustomConfig(EntityTypeBuilder<UserCount> builder)
        {
            
        }
    }
}
