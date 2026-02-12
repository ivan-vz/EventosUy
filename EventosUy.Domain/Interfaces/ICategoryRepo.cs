using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface ICategoryRepo
    {
        public Task<Category?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<bool> ExistsAsync(string name);
        public Task AddAsync(Category category);
        public void Update(Category category);
        public Task Save();
    }
}
