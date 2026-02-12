using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EventRepo : IEventRepo
    {
        private readonly ApplicationDbContext _context;

        public EventRepo(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task AddAsync(Event ev) => await _context.Event.AddAsync(ev);

        public async Task<bool> ExistsByInitialsAsync(string initials) => await _context.Event.AnyAsync(x => x.Initials == initials);

        public async Task<bool> ExistsByNameAsync(string name) => await _context.Event.AnyAsync(x => x.Name == name);

        public async Task<IEnumerable<Event>> GetAllAsync() => await _context.Event.ToListAsync();

        public async Task<IEnumerable<Event>> GetAllByInstitutionAsync(Guid institutionId) => await _context.Event.Where(x => x.InstitutionId == institutionId).ToListAsync();

        public async Task<Event?> GetByIdAsync(Guid id) => await _context.Event.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Event ev)
        {
            _context.Event.Attach(ev);
            _context.Event.Entry(ev).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
