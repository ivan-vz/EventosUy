using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nickname).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Password).HasMaxLength(500).IsRequired();

            builder.Property(x => x.Email).HasColumnType("citext").HasMaxLength(255).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Active).IsRequired();

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Acronym).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Description).IsRequired();

            builder.Property(x => x.Url).HasColumnType("citext").HasMaxLength(255).IsRequired();

            builder.Property(x => x.Country).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.City).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Street).HasColumnType("citext").HasMaxLength(200).IsRequired();

            builder.Property(x => x.Number).HasMaxLength(5).IsRequired();

            builder.Property(x => x.Floor).IsRequired();

            builder.HasIndex(x => x.Nickname).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Email).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Acronym).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Url).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => new { x.Country, x.City, x.Street, x.Number, x.Floor }).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
