using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterTypeRepo
    {
        public Task<RegisterType> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByEdition(Guid editionId);
        public Task AddAsync(RegisterType registerType);
        public Task RemoveAsync(Guid id);
    }
}
