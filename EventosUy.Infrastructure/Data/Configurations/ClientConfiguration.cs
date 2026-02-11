using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder) 
        {
            builder.ToTable("Clients");

            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(x => x.LastName).HasMaxLength(100);

            builder.Property(x => x.FirstSurname).HasMaxLength(100).IsRequired();

            builder.Property(x => x.LastSurname).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Birthday).IsRequired();

            builder.Property(x => x.Ci).HasMaxLength(8).IsRequired();

            builder.HasIndex(x => x.Ci).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
