using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class RegisterTypeRepo : IRegisterTypeRepo
    {
        private readonly ApplicationDbContext _context;

        public RegisterTypeRepo(ApplicationDbContext context) 
        { 
            _context = context; 
        }

        public async Task AddAsync(RegisterType registerType) => await _context.RegisterTypes.AddAsync(registerType);

        public async Task<bool> ExistsAsync(string name) => await _context.RegisterTypes.AnyAsync(x => x.Name == name);

        public async Task<IEnumerable<RegisterType>> GetAllByEditionAsync(Guid editionId) => await _context.RegisterTypes.Where(x => x.EditionId == editionId).ToListAsync();

        public async Task<RegisterType?> GetByIdAsync(Guid id) => await _context.RegisterTypes.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(RegisterType registerType)
        {
            _context.RegisterTypes.Attach(registerType);
            _context.RegisterTypes.Entry(registerType).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
