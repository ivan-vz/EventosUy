using EventosUy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<RegisterType> RegisterTypes { get; set; }
        public DbSet<Sponsorship> Sponsorships { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Register> Registers { get; set; }
    }
}
