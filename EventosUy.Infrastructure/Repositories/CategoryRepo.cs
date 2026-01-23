using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class CategoryRepo : ICategoryRepo
    {
        private readonly HashSet<Category> _categories;

        public CategoryRepo() { _categories = []; }

        public Task AddAsync(Category category)
        {
            _categories.Add(category);
            return Task.CompletedTask;
        }
        
        public Task<bool> ExistsAsync(string name) 
        { 
            return Task.FromResult(_categories.Any(category => category.Active && category.Name.Equals(name, StringComparison.OrdinalIgnoreCase))); 
        }

        public Task<List<Category>> GetAllAsync() { return Task.FromResult(_categories.Where(category => category.Active).ToList()); }

        public Task<Category?> GetByIdAsync(Guid id) { return Task.FromResult(_categories.SingleOrDefault(category => category.Active && category.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _categories.RemoveWhere(category => category.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
