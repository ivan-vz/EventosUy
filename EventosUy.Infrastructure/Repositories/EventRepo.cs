using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EventRepo : IEventRepo
    {
        private readonly HashSet<Event> _events;

        public EventRepo() { _events = []; }

        public Task AddAsync(Event eventInstance) { return Task.FromResult(_events.Add(eventInstance)); }
        
        public Task<bool> ExistsAsync(string name) { return Task.FromResult(_events.Any(eventInstance => eventInstance.Name.Equals(name, StringComparison.OrdinalIgnoreCase))); }

        public Task<List<Guid>> GetAllAsync() { return Task.FromResult(_events.Select(eventInstance => eventInstance.Id).ToList()); }

        public Task<List<Guid>> GetAllByInstitutionAsync(Guid institutionId) 
        {
            return Task.FromResult(_events.Where(eventInstance => eventInstance.Institution == institutionId).Select(eventInstance => eventInstance.Id).ToList());
        }

        public Task<Event?> GetByIdAsync(Guid id) { return Task.FromResult(_events.SingleOrDefault(eventInstance => eventInstance.Id == id)); }

        public Task<bool> RemoveAsync(Guid id) 
        {
            int result = _events.RemoveWhere(eventInstance => eventInstance.Id == id);
            return Task.FromResult(result > 0); 
        }
    }
}
