using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEventRepo
    {
        public Task<Event> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByInstitutionAsync(Guid institutionId);
        public IEnumerable<Guid> GetCategoriesAsync(Guid id);
        public Task AddAsync(Event eventInstance);
        public Task RemoveAsync(Guid id);
    }
}
