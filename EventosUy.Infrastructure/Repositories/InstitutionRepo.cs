using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class InstitutionRepo : IInstitutionRepo
    {
        private readonly ApplicationDbContext _context;

        public InstitutionRepo(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task AddAsync(Institution institution) => await _context.Institutions.AddAsync(institution);

        public async Task<bool> ExistsByAcronymAsync(string acronym) => await _context.Institutions.AnyAsync(x => x.Acronym == acronym); 
        

        public async Task<bool> ExistsByAddressAsync(string country, string city, string street, string number, int floor) => await _context.Institutions.AnyAsync(x =>
                x.Country == country 
                && x.City == city
                && x.Street == street
                && x.Number == number
                && x.Floor == floor
                );

        public async Task<bool> ExistsByEmailAsync(string email) => await _context.Institutions.AnyAsync(x => x.Email == email);
        
        public async Task<bool> ExistsByNicknameAsync(string nickname)  => await _context.Institutions.AnyAsync(x => x.Nickname == nickname);

        public async Task<bool> ExistsByUrlAsync(string url) => await _context.Institutions.AnyAsync(x => x.Url == url);

        public async Task<IEnumerable<Institution>> GetAllAsync() => await _context.Institutions.ToListAsync();

        public async Task<Institution?> GetByIdAsync(Guid id) => await _context.Institutions.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Institution institution)
        {
            _context.Institutions.Attach(institution);
            _context.Institutions.Entry(institution).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
