using EventosUy.Domain.Entidades;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterRepo
    {
        public Task<Register?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllByPersonAsync(Guid personId);
        public Task<List<Guid>> GetAllByEditionAsync(Guid editionId);
        public Task<bool> ExistsAsync(Guid personId, Guid editionId);
        public Task AddAsync(Register register);
        public Task<bool> RemoveAsync(Guid id);
    }
}
