using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IClientRepo
    {
        public Task<Client?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Client>> GetAllAsync();
        public Task<bool> ExistsByNicknameAsync(string nickname);
        public Task<bool> ExistsByEmailAsync(string email);
        public Task<bool> ExistsByCiAsync(string ci);
        public Task AddAsync(Client person);
        public void Update(Client client);
        public Task Save();
    }
}
