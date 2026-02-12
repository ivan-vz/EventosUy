using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nickname).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Password).HasMaxLength(500).IsRequired();

            builder.Property(x => x.Email).HasColumnType("citext").HasMaxLength(255).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Active).IsRequired();

            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(x => x.LastName).HasMaxLength(100);

            builder.Property(x => x.FirstSurname).HasMaxLength(100).IsRequired();

            builder.Property(x => x.LastSurname).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Birthday).IsRequired();

            builder.Property(x => x.Ci).HasMaxLength(8).IsRequired();

            builder.HasIndex(x => x.Nickname).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Email).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Ci).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
