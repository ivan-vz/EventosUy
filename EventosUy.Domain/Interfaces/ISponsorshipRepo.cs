using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface ISponsorshipRepo
    {
        public Task<Sponsorship?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllByEdition(Guid editionId);
        public Task<List<Guid>> GetAllByInstitution(Guid institutionId);
        public Task<bool> ExistsAsync(Guid editionId, Guid institutionId);
        public Task AddAsync(Sponsorship sponsorship);
        public Task<bool> RemoveAsync(Guid id);
    }
}
