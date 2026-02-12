using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Initials).HasColumnType("citext").HasMaxLength(10).IsRequired();

            builder.Property(x => x.Description).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Categories).HasColumnType("text[]").IsRequired();

            builder.Property(x => x.InstitutionId).IsRequired();

            builder.HasOne<Institution>().WithMany().HasForeignKey(x => x.InstitutionId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Active).IsRequired();

            builder.HasIndex(x => x.InstitutionId).HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Initials).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Name).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
