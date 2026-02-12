using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Client> Clients { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<RegisterType> RegisterTypes { get; set; }
        public DbSet<Sponsorship> Sponsorships { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Register> Registers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Client>().HasQueryFilter(x => x.Active);

            modelBuilder.Entity<Institution>().HasQueryFilter(x => x.Active);

            modelBuilder.Entity<Category>().HasQueryFilter(x => x.Active);

            modelBuilder.Entity<Event>().HasQueryFilter(x => x.Active);

            modelBuilder.Entity<Edition>().HasQueryFilter(x => EditionState.PENDING != x.State && EditionState.CANCELLED != x.State);

            modelBuilder.Entity<RegisterType>().HasQueryFilter(x => x.Active);

            modelBuilder.Entity<Sponsorship>().HasQueryFilter(x => x.Active);

            modelBuilder.Entity<Voucher>().HasQueryFilter(x => VoucherState.AVAILABLE == x.State);
        }
    }
}
