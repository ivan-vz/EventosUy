using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEditionRepo
    {
        public Task<Edition?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllAsync();
        public Task<List<Guid>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<List<Guid>> GetAllByEventAsync(Guid eventId);
        public Task<List<Guid>> GetAllPendingByEvent(Guid eventId);
        public Task<bool> AlreadyExists(string name);
        public Task AddAsync(Edition edition);
        public Task RemoveAsync(Guid id);
    }
}
