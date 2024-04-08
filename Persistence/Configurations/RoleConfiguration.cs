using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p => p.Id).HasMaxLength(256);
            builder.Property(x => x.Id)
              .HasValueGenerator<SeqIdValueGenerator>()
              .ValueGeneratedOnAdd();
        }
    }
}