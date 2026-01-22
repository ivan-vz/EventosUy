using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.Common;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EventRepo : IEventRepo
    {
        private readonly HashSet<Event> _events;

        public EventRepo() { _events = []; }

        public Task AddAsync(Event eventInstance) { return Task.FromResult(_events.Add(eventInstance)); }

        public Task<List<Event>> GetAllAsync() { return Task.FromResult(_events.ToList()); }

        public Task<List<Event>> GetAllByInstitutionAsync(Guid institutionId) { return Task.FromResult(_events.Where(eventInstance => eventInstance.Institution == institutionId).ToList()); }

        public Task<Event?> GetByIdAsync(Guid id) { return Task.FromResult(_events.SingleOrDefault(eventInstance => eventInstance.Id == id)); }

        public Task<bool> RemoveAsync(Guid id) 
        {
            int result = _events.RemoveWhere(eventInstance => eventInstance.Id == id);
            return Task.FromResult(result > 0); 
        }

        public Task<ValidationResult> ValidateAsync(string name, string initials)
        {
            var result = new ValidationResult();

            foreach (var item in _events) 
            {
                if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) { result.AddDuplicate(DuplicateField.NAME); }

                if (item.Initials.Equals(initials, StringComparison.OrdinalIgnoreCase)) { result.AddDuplicate(DuplicateField.INITIALS); }

                if (result.HasDuplicate(DuplicateField.NAME) && result.HasDuplicate(DuplicateField.INITIALS)) { break; }
            }

            return Task.FromResult(result);
        }
    }
}
