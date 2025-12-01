using EventosUy.Domain.Entidades;

namespace EventosUy.Domain.Interfaces
{
    public interface IRegisterRepo
    {
        public Task<Register> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByPersonAsync(Guid personId);
        public IEnumerable<Guid> GetAllByEditionAsync(Guid editionId);
        public IEnumerable<Guid> GetAllRegisterTypeAsync(Guid registerTypeId);
        public Task AddAsync(Register register);
        public Task RemoveAsync(Guid id);
    }
}
