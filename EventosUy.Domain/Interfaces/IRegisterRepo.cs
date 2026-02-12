using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterRepo
    {
        public Task<Register?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Register>> GetAllByClientAsync(Guid clientId);
        public Task<IEnumerable<Register>> GetAllByEditionAsync(Guid editionId);
        public Task<bool> ExistsAsync(Guid personId, Guid editionId);
        public Task AddAsync(Register register);
        public void Update(Register register);
        public Task Save();
    }
}
