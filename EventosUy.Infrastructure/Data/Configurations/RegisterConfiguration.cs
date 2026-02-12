using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosUy.Infrastructure.Data.Configurations
{
    public class RegisterConfiguration : IEntityTypeConfiguration<Register>
    {
        public void Configure(EntityTypeBuilder<Register> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Total).IsRequired();

            builder.Property(x => x.Created).IsRequired();

            builder.Property(x => x.ClientId);

            builder.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.EditionId).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.EditionId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RegisterTypeId).IsRequired();

            builder.HasOne<RegisterType>().WithMany().HasForeignKey(x => x.RegisterTypeId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.VoucherId);

            builder.HasOne<Voucher>().WithMany().HasForeignKey(x => x.VoucherId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.ClientId, x.EditionId });

            builder.HasIndex(x => x.ClientId);

            builder.HasIndex(x => x.EditionId);
        }
    }
}
