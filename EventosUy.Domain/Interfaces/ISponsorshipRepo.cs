using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface ISponsorshipRepo
    {
        public Task<Sponsorship> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByEdition(Guid editionId);
        public IEnumerable<Guid> GetAllByInstitution(Guid institutionId);
        public IEnumerable<Guid> GetAllByRegisterType(Guid registerTypeId);
        public Task AddAsync(Sponsorship sponsorship);
        public Task RemoveAsync(Guid id);
    }
}
