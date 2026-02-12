using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface ISponsorshipRepo
    {
        public Task<Sponsorship?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Sponsorship>> GetAllByEditionAsync(Guid editionId);
        public Task<IEnumerable<Sponsorship>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<bool> ExistsAsync(Guid editionId, Guid institutionId);
        public Task AddAsync(Sponsorship sponsorship);
        public void Update(Sponsorship sponsor);
        public Task Save();
    }
}
