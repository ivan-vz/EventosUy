using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class SponsorshipConfiguration : IEntityTypeConfiguration<Sponsorship>
    {
        public void Configure(EntityTypeBuilder<Sponsorship> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.Tier).IsRequired();

            builder.Property(x => x.Created).IsRequired();
            
            builder.Property(x => x.Active).IsRequired();

            builder.Property(x => x.InstitutionId).IsRequired();

            builder.HasOne<Institution>().WithMany().HasForeignKey(x => x.InstitutionId).OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(x => x.EditionId).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.EditionId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RegisterTypeId).IsRequired();

            builder.HasOne<RegisterType>().WithMany().HasForeignKey(x => x.RegisterTypeId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.InstitutionId);
            builder.HasIndex(x => x.EditionId);

            builder.HasIndex(x => new { x.InstitutionId, x.EditionId }).HasFilter("\"Active\" = 'true'");
        }
    }
}
