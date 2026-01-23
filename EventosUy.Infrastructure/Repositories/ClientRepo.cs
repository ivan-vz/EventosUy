using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class ClientRepo : IClientRepo
    {
        private readonly HashSet<Client> _clients;

        public ClientRepo() { _clients = []; }

        public Task AddAsync(Client client)
        {
            _clients.Add(client);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(string email) 
        { 
            return Task.FromResult(_clients.Any(client => client.Active && client.Email.Equals(email, StringComparison.OrdinalIgnoreCase))); 
        }

        public Task<bool> ExistsByNicknameAsync(string nickname) { return Task.FromResult(_clients.Any(client => client.Active && client.Nickname == nickname)); }

        public Task<List<Client>> GetAllAsync() { return Task.FromResult(_clients.Where(client => client.Active).ToList()); }

        public Task<Client?> GetByIdAsync(Guid id) { return Task.FromResult(_clients.SingleOrDefault(client => client.Active && client.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _clients.RemoveWhere(client => client.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
