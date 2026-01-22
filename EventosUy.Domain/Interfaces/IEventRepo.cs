using EventosUy.Domain.Entities;
using EventosUy.Domain.Common;

namespace EventosUy.Domain.Interfaces
{
    public interface IEventRepo
    {
        public Task<Event?> GetByIdAsync(Guid id);
        public Task<List<Event>> GetAllAsync();
        public Task<List<Event>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<ValidationResult> ValidateAsync(string name, string initials);
        public Task AddAsync(Event eventInstance);
        public Task<bool> RemoveAsync(Guid id);
    }
}
