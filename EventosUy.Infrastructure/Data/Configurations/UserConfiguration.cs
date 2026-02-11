using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nickname).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Password).HasMaxLength(500).IsRequired();

            builder.Property(x => x.Email).HasMaxLength(255).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.Active).IsRequired();

            builder.HasIndex(x => x.Nickname).IsUnique().HasFilter("\"Active\" = true");
            builder.HasIndex(x => x.Email).IsUnique().HasFilter("\"Active\" = true");
        }
    }
}
