using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEditionRepo
    {
        public Task<Edition?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Edition>> GetAllAsync();
        public Task<IEnumerable<Edition>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<IEnumerable<Edition>> GetAllByEventAsync(Guid eventId);
        public Task<IEnumerable<Edition>> GetAllPendingByEventAsync(Guid eventId);
        public Task<bool> ExistsByNameAsync(string name);
        public Task<bool> ExistsByInitialsAsync(string initials);
        public Task<bool> ExistsEventAt(string country, string city, string street, string number, int floor, DateOnly from);
        public Task AddAsync(Edition edition);
        public void Update(Edition edition);
        public Task Save();
    }
}
