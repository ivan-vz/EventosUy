using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class RegisterRepo : IRegisterRepo
    {
        private readonly ApplicationDbContext _context;

        public RegisterRepo(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task AddAsync(Register register) => await _context.Registers.AddAsync(register);

        public async Task<bool> ExistsAsync(Guid clientId, Guid editionId) => await _context.Registers.AnyAsync(x => x.ClientId == clientId && x.EditionId == editionId);

        public async Task<IEnumerable<Register>> GetAllByEditionAsync(Guid editionId) => await _context.Registers.Where(x => x.EditionId == editionId).ToListAsync();

        public async Task<IEnumerable<Register>> GetAllByClientAsync(Guid clientId) => await _context.Registers.Where(x => x.ClientId == clientId).ToListAsync();

        public async Task<Register?> GetByIdAsync(Guid id) => await _context.Registers.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Register register)
        {
            _context.Registers.Attach(register);
            _context.Registers.Entry(register).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
