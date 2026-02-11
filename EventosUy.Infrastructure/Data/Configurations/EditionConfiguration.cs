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

            builder.Property(x => x.Name).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Initials).HasMaxLength(10).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.From).IsRequired();

            builder.Property(x => x.To).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Country).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.City).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Street).HasMaxLength(200).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Number).HasMaxLength(5).IsRequired();

            builder.Property(x => x.Floor).IsRequired();

            builder.Property(x => x.State).HasConversion<string>().IsRequired();
            
            builder.Property(x => x.Event).IsRequired();

            builder.HasOne<Event>().WithMany().HasForeignKey(x => x.Event).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Institution).IsRequired();

            builder.HasOne<Institution>().WithMany().HasForeignKey(x => x.Institution).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Institution);
            builder.HasIndex(x => x.Event);
            builder.HasIndex(x => x.State);

            builder.HasIndex(x => new { x.Institution, x.State });
            builder.HasIndex(x => new { x.Event, x.State });

            builder.HasIndex(x => x.Initials).IsUnique().HasFilter("\"State\" = 'CANCELLED'");
            builder.HasIndex(x => x.Name).IsUnique().HasFilter("\"State\" = 'CANCELLED'");
            builder.HasIndex(x => new { x.Country, x.City, x.Street, x.Number, x.Floor, x.To }).HasFilter("\"State\" = 'CANCELLED'");
        }
    }
}
