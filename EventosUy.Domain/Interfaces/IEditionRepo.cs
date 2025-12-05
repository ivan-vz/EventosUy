using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEditionRepo
    {
        public Task<Edition?> GetByIdAsync(Guid id);
        public Task<List<Edition>> GetAllAsync();
        public Task<List<Edition>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<List<Edition>> GetAllByEventAsync(Guid eventId);
        public Task<List<Edition>> GetAllPendingByEventAsync(Guid eventId);
        public Task<bool> ExistsAsync(string name);
        public Task AddAsync(Edition edition);
        public Task<bool> RemoveAsync(Guid id);
    }
}
