using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepo(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task AddAsync(Category category) => await _context.Categories.AddAsync(category);
        
        public async Task<bool> ExistsAsync(string name) => await _context.Categories.AnyAsync(x => x.Name == name);

        public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.ToListAsync();

        public async Task<Category?> GetByIdAsync(Guid id) => await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Category category)
        {
            _context.Categories.Attach(category);
            _context.Categories.Entry(category).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
