using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class ClientRepo : IClientRepo
    {
        private readonly HashSet<Client> _clients;

        private ApplicationDbContext _context;

        public ClientRepo(ApplicationDbContext context) 
        { 
            _clients = []; 
            _context = context;
        }

        public async Task AddAsync(Client client) => await _context.Clients.AddAsync(client);

        public async Task<bool> ExistsByEmailAsync(string email) => await _context.Clients.AnyAsync(x => x.Email == email);

        public async Task<bool> ExistsByNicknameAsync(string nickname) => await _context.Clients.AnyAsync(x => x.Nickname == nickname);

        public async Task<bool> ExistsByCiAsync(string ci) => await _context.Clients.AnyAsync(x => x.Ci == ci);

        public async Task<IEnumerable<Client>> GetAllAsync() => await _context.Clients.ToListAsync();

        public async Task<Client?> GetByIdAsync(Guid id) => await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Client client) 
        {
            _context.Clients.Attach(client);
            _context.Clients.Entry(client).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
