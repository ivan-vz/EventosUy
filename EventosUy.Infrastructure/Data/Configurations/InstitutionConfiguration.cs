using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> builder)
        {
            builder.ToTable("Institutions");

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Acronym).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Description).IsRequired();

            builder.Property(x => x.Url).HasMaxLength(255).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Country).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.City).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Street).HasMaxLength(200).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Number).HasMaxLength(5).IsRequired();

            builder.Property(x => x.Floor).IsRequired();

            builder.Property(x => x.Active).IsRequired();

            builder.HasIndex(x => x.Acronym).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Url).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => new { x.Country, x.City, x.Street, x.Number, x.Floor }).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
