using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Interfaces
{
    public interface IPersonRepo
    {
        public Task<Client?> GetByIdAsync(Guid id);
        public Task<List<Client>> GetAllAsync();
        public Task<List<Client>> GetAllExceptAsync(List<Guid> ids);
        public Task<bool> ExistsByNicknameAsync(string nickname);
        public Task<bool> ExistsByEmailAsync(Email email);
        public Task AddAsync(Client person);
        public Task<bool> RemoveAsync(Guid id);
    }
}
