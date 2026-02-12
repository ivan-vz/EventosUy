using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EditionRepo : IEditionRepo
    {
        private readonly ApplicationDbContext _context;

        public EditionRepo(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task AddAsync(Edition edition) => await _context.Editions.AddAsync(edition);

        public async Task<bool> ExistsByInitialsAsync(string initials) => await _context.Editions.AnyAsync(x => x.Initials == initials);

        public async Task<bool> ExistsByNameAsync(string name) => await _context.Editions.AnyAsync(x => x.Name == name);

        public async Task<bool> ExistsEventAt(string country, string city, string street, string number, int floor, DateOnly from) => await _context.Editions.AnyAsync(x =>
                x.Country == country
                && x.City == city
                && x.Street == street
                && x.Number == number
                && x.Floor == floor
                && x.To >= from );

        public async Task<IEnumerable<Edition>> GetAllAsync() => await _context.Editions.ToListAsync();

        public async Task<IEnumerable<Edition>> GetAllByEventAsync(Guid eventId) => await _context.Editions.Where(x => x.EventId == eventId).ToListAsync();

        public async Task<IEnumerable<Edition>> GetAllByInstitutionAsync(Guid institutionId) => await _context.Editions.Where(x => x.InstitutionId == institutionId).ToListAsync();

        public async Task<IEnumerable<Edition>> GetAllPendingByEventAsync(Guid eventId) => await _context.Editions.IgnoreQueryFilters()
            .Where(x => 
                x.EventId == eventId 
                && x.State == EditionState.PENDING
            ).ToListAsync();

        public async Task<Edition?> GetByIdAsync(Guid id) => await _context.Editions.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Edition edition)
        {
            _context.Editions.Attach(edition);
            _context.Editions.Entry(edition).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
