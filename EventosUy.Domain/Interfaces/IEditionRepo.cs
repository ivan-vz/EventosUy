using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEditionRepo
    {
        public Task<Edition> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByInstitutionAsync(Guid institutionId);
        public IEnumerable<Guid> GetAllByEventAsync(Guid eventId);
        public Task AddAsync(Edition edition);
        public Task RemoveAsync(Guid id);
    }
}
