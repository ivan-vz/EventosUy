using EventosUy.Domain.Entidades;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class RegisterRepo : IRegisterRepo
    {
        private readonly HashSet<Register> _registers;

        public RegisterRepo() { _registers = []; }

        public Task AddAsync(Register register)
        {
            _registers.Add(register);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid personId, Guid editionId) { return Task.FromResult(_registers.Any(register => register.Person == personId && register.Edition == editionId)); }

        public Task<List<Register>> GetAllByEditionAsync(Guid editionId) { return Task.FromResult(_registers.Where(register => register.Edition == editionId).ToList()); }

        public Task<List<Register>> GetAllByPersonAsync(Guid personId) { return Task.FromResult(_registers.Where(register => register.Person == personId).ToList()); }

        public Task<Register?> GetByIdAsync(Guid id) { return Task.FromResult(_registers.SingleOrDefault(register => register.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _registers.RemoveWhere(register => register.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
