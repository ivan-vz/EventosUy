using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EventRepo : IEventRepo
    {
        private readonly HashSet<Event> _events;

        public EventRepo() { _events = []; }

        public Task AddAsync(Event ev) { return Task.FromResult(_events.Add(ev)); }

        public Task<bool> ExistsByInitialsAsync(string initials)
        {
            return Task.FromResult( _events.Any(ev => ev.Active && ev.Initials.Equals(initials, StringComparison.OrdinalIgnoreCase)) );
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            return Task.FromResult(_events.Any(ev => ev.Active && ev.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<List<Event>> GetAllAsync() { return Task.FromResult(_events.Where(ev => ev.Active).ToList()); }

        public Task<List<Event>> GetAllByInstitutionAsync(Guid institutionId) 
        { 
            return Task.FromResult(_events.Where(ev => ev.Active && ev.InstitutionId == institutionId).ToList()); 
        }

        public Task<Event?> GetByIdAsync(Guid id) { return Task.FromResult(_events.SingleOrDefault(ev => ev.Active && ev.Id == id)); }

        public Task<bool> RemoveAsync(Guid id) 
        {
            int result = _events.RemoveWhere(ev => ev.Id == id);
            return Task.FromResult(result > 0); 
        }
    }
}
