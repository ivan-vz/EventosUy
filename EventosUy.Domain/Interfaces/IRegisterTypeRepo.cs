using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterTypeRepo
    {
        public Task<RegisterType?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllByEdition(Guid editionId);
        public Task<bool> ExistsAsync(string name);
        public Task AddAsync(RegisterType registerType);
        public Task<bool> RemoveAsync(Guid id);
    }
}
