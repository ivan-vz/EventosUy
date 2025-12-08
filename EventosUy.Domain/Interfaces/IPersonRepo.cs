using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Interfaces
{
    public interface IPersonRepo
    {
        public Task<Person?> GetByIdAsync(Guid id);
        public Task<List<Person>> GetAllAsync();
        public Task<List<Person>> GetAllExceptAsync(List<Guid> ids);
        public Task<bool> ExistsByNicknameAsync(string nickname);
        public Task<bool> ExistsByEmailAsync(Email email);
        public Task AddAsync(Person person);
        public Task<bool> RemoveAsync(Guid id);
    }
}
