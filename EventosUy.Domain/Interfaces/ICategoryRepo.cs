using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface ICategoryRepo
    {
        public Task<Category> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public Task AddAsync(Category category);
        public Task RemoveAsync(Guid id);
    }
}
