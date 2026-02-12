using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterTypeRepo
    {
        public Task<RegisterType?> GetByIdAsync(Guid id);
        public Task<IEnumerable<RegisterType>> GetAllByEditionAsync(Guid editionId);
        public Task<bool> ExistsAsync(string name);
        public Task AddAsync(RegisterType registerType);
        public void Update(RegisterType registerType);
        public Task Save();
    }
}
