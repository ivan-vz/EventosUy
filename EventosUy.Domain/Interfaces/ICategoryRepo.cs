using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface ICategoryRepo
    {
        public Task<Category?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllAsync();
        public Task<bool> ExistsAsync(string name);
        public Task AddAsync(Category category);
        public Task<bool> RemoveAsync(Guid id);
    }
}
