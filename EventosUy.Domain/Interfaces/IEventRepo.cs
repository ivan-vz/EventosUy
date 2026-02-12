using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEventRepo
    {
        public Task<bool> ExistsByNameAsync(string name);
        public Task<bool> ExistsByInitialsAsync(string initials);
        public Task<Event?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Event>> GetAllAsync();
        public Task<IEnumerable<Event>> GetAllByInstitutionAsync(Guid institutionId);
        public Task AddAsync(Event eventInstance);
        public void Update(Event ev);
        public Task Save();
    }
}
