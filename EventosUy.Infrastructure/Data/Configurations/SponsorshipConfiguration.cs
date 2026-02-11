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

            builder.Property(x => x.Name).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.Tier).IsRequired();

            builder.Property(x => x.Created).IsRequired();
            
            builder.Property(x => x.Active).IsRequired();

            builder.Property(x => x.Institution).IsRequired();

            builder.HasOne<Institution>().WithMany().HasForeignKey(x => x.Institution).OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(x => x.Edition).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.Edition).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RegisterType).IsRequired();

            builder.HasOne<RegisterType>().WithMany().HasForeignKey(x => x.RegisterType).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Institution);
            builder.HasIndex(x => x.Edition);

            builder.HasIndex(x => new { x.Institution, x.Edition }).HasFilter("\"Active\" = 'true'");
        }
    }
}
