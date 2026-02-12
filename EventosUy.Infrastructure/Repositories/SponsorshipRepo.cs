using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class SponsorshipRepo : ISponsorshipRepo
    {
        private readonly ApplicationDbContext _context;

        public SponsorshipRepo(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task AddAsync(Sponsorship sponsorship) => await _context.Sponsorships.AddAsync(sponsorship);

        public async Task<bool> ExistsAsync(Guid editionId, Guid institutionId) => await _context.Sponsorships.AnyAsync(x => x.InstitutionId == institutionId && x.EditionId == editionId);

        public async Task<IEnumerable<Sponsorship>> GetAllByEditionAsync(Guid editionId) => await _context.Sponsorships.Where(x => x.EditionId == editionId).ToListAsync();

        public async Task<IEnumerable<Sponsorship>> GetAllByInstitutionAsync(Guid institutionId) => await _context.Sponsorships.Where(x => x.InstitutionId == institutionId).ToListAsync();

        public async Task<Sponsorship?> GetByIdAsync(Guid id) => await _context.Sponsorships.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Sponsorship sponsor)
        {
            _context.Sponsorships.Attach(sponsor);
            _context.Sponsorships.Entry(sponsor).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
