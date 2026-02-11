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

            builder.Property(x => x.Name).HasMaxLength(100).UseCollation("case_insensitive").IsRequired();

            builder.Property(x => x.Code).IsRequired();

            builder.Property(x => x.Discount).IsRequired();

            builder.Property(x => x.Quota).IsRequired();

            builder.Property(x => x.Used).IsRequired();

            builder.Property(x => x.IsAutoApplied).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.State).HasConversion<string>().IsRequired();

            builder.Property(x => x.Edition).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.Edition).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RegisterType).IsRequired();

            builder.HasOne<RegisterType>().WithMany().HasForeignKey(x => x.RegisterType).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Sponsor);

            builder.HasOne<Sponsorship>().WithMany().HasForeignKey(x => x.Sponsor).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code).HasFilter("\"State\" = 'AVAILABLE'");
        }
    }
}
