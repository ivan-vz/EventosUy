using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasColumnType("citext").HasMaxLength(100).IsRequired();

            builder.Property(x => x.Code).IsRequired();

            builder.Property(x => x.Discount).IsRequired();

            builder.Property(x => x.Quota).IsRequired();

            builder.Property(x => x.Used).IsRequired();

            builder.Property(x => x.IsAutoApplied).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.State).HasConversion<string>().IsRequired();

            builder.Property(x => x.EditionId).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.EditionId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RegisterTypeId).IsRequired();

            builder.HasOne<RegisterType>().WithMany().HasForeignKey(x => x.RegisterTypeId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.SponsorId);

            builder.HasOne<Sponsorship>().WithMany().HasForeignKey(x => x.SponsorId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code).HasFilter("\"State\" = 'AVAILABLE'");
        }
    }
}
