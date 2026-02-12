using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class EditionConfiguration : IEntityTypeConfiguration<Edition>
    {
        public void Configure(EntityTypeBuilder<Edition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Initials).HasColumnType("citext").HasMaxLength(10).IsRequired();

            builder.Property(x => x.From).IsRequired();

            builder.Property(x => x.To).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Country).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.City).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Street).HasColumnType("citext").HasMaxLength(200).IsRequired();

            builder.Property(x => x.Number).HasMaxLength(5).IsRequired();

            builder.Property(x => x.Floor).IsRequired();

            builder.Property(x => x.State).HasConversion<string>().IsRequired();
            
            builder.Property(x => x.EventId).IsRequired();

            builder.HasOne<Event>().WithMany().HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.InstitutionId).IsRequired();

            builder.HasOne<Institution>().WithMany().HasForeignKey(x => x.InstitutionId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.InstitutionId);
            builder.HasIndex(x => x.EventId);
            builder.HasIndex(x => x.State);

            builder.HasIndex(x => new { x.InstitutionId, x.State });
            builder.HasIndex(x => new { x.EventId, x.State });

            builder.HasIndex(x => x.Initials).IsUnique().HasFilter("\"State\" = 'CANCELLED'");
            builder.HasIndex(x => x.Name).IsUnique().HasFilter("\"State\" = 'CANCELLED'");
            builder.HasIndex(x => new { x.Country, x.City, x.Street, x.Number, x.Floor, x.To }).HasFilter("\"State\" = 'CANCELLED'");
        }
    }
}
