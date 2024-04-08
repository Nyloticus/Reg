using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations
{
    public abstract class BaseConfiguration<T, TId> : IEntityTypeConfiguration<T> where T : BaseEntity<TId> 
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(c => c.Id);            

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();
            this.CustomConfig(builder);

        }
        public abstract void CustomConfig(EntityTypeBuilder<T> builder);
    }
}
