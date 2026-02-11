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

            builder.Property(x => x.Client);

            builder.HasOne<Client>().WithMany().HasForeignKey(x => x.Client).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Edition).IsRequired();

            builder.HasOne<Edition>().WithMany().HasForeignKey(x => x.Edition).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RegisterType).IsRequired();

            builder.HasOne<RegisterType>().WithMany().HasForeignKey(x => x.RegisterType).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Voucher);

            builder.HasOne<Voucher>().WithMany().HasForeignKey(x => x.Voucher).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.Client, x.Edition });

            builder.HasIndex(x => x.Client);

            builder.HasIndex(x => x.Edition);
        }
    }
}
