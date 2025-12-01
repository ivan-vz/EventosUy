using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IPersonRepo
    {
        public Task<Person> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public Task AddAsync(Person person);
        public Task RemoveAsync(Guid id);
    }
}
