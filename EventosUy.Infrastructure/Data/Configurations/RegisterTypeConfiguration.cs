using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class RegisterTypeConfiguration : IEntityTypeConfiguration<RegisterType>
    {
        public void Configure(EntityTypeBuilder<RegisterType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Description).IsRequired();

            builder.Property(x => x.Price).IsRequired();

            builder.Property(x => x.Quota).IsRequired();

            builder.Property(x => x.Used).IsRequired();
            
            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Active).IsRequired();

            builder.Property(x => x.EditionId).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.EditionId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.EditionId).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
