using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEventRepo
    {
        public Task<Event?> GetByIdAsync(Guid id);
        public Task<List<Event>> GetAllAsync();
        public Task<List<Event>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<bool> ExistsAsync(string name); 
        public Task AddAsync(Event eventInstance);
        public Task<bool> RemoveAsync(Guid id);
    }
}
