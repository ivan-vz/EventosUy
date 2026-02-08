using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterRepo
    {
        public Task<Register?> GetByIdAsync(Guid id);
        public Task<List<Register>> GetAllByClientAsync(Guid clientId);
        public Task<List<Register>> GetAllByEditionAsync(Guid editionId);
        public Task<bool> ExistsAsync(Guid personId, Guid editionId);
        public Task AddAsync(Register register);
        public Task<bool> RemoveAsync(Guid id);
    }
}
