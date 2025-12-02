using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositorios
{
    internal class RegisterTypeRepo : IRegisterTypeRepo
    {
        private readonly HashSet<RegisterType> _registerTypes;

        public RegisterTypeRepo() { _registerTypes = []; }

        public Task AddAsync(RegisterType registerType)
        {
            _registerTypes.Add(registerType);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string name) { return Task.FromResult(_registerTypes.Any(registerType => registerType.Name.Equals(name))); }

        public Task<List<Guid>> GetAllByEdition(Guid editionId) { return Task.FromResult(_registerTypes.Where(registerType => registerType.Edition == editionId).Select(registerType => registerType.Id).ToList()); }

        public Task<RegisterType?> GetByIdAsync(Guid id) { return Task.FromResult(_registerTypes.SingleOrDefault(registerType => registerType.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _registerTypes.RemoveWhere(registerType => registerType.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
