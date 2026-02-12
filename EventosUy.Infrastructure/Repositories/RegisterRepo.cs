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

        public Task<bool> ExistsAsync(Guid clientId, Guid editionId) { return Task.FromResult(_registers.Any(register => register.ClientId == clientId && register.EditionId == editionId)); }

        public Task<List<Register>> GetAllByEditionAsync(Guid editionId) { return Task.FromResult(_registers.Where(register => register.EditionId == editionId).ToList()); }

        public Task<List<Register>> GetAllByClientAsync(Guid clientId) { return Task.FromResult(_registers.Where(register => register.ClientId == clientId).ToList()); }

        public Task<Register?> GetByIdAsync(Guid id) { return Task.FromResult(_registers.SingleOrDefault(register => register.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _registers.RemoveWhere(register => register.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
