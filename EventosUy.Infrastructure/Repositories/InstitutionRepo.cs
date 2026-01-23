using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class InstitutionRepo : IInstitutionRepo
    {
        private readonly HashSet<Institution> _institutions;

        public InstitutionRepo() { _institutions = []; }

        public Task AddAsync(Institution institution)
        {
            _institutions.Add(institution);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByAcronymAsync(string acronym) 
        { 
            return Task.FromResult(_institutions.Any(institution => institution.Active && institution.Acronym.Equals(acronym, StringComparison.OrdinalIgnoreCase))); 
        }

        public Task<bool> ExistsByAddressAsync(string country, string city, string street, string number, int floor) 
        { 
            return Task.FromResult(_institutions.Any(institution =>
                institution.Active && 
                institution.Country.Equals(country, StringComparison.OrdinalIgnoreCase) &&
                institution.City.Equals(city, StringComparison.OrdinalIgnoreCase) &&
                institution.Street.Equals(street, StringComparison.OrdinalIgnoreCase) &&
                institution.Number == number &&
                institution.Floor == floor
                )); 
        }

        public Task<bool> ExistsByEmailAsync(string email) 
        { 
            return Task.FromResult(_institutions.Any(institution => institution.Active && institution.Email.Equals(email, StringComparison.OrdinalIgnoreCase))); 
        }
        
        public Task<bool> ExistsByNicknameAsync(string nickname) 
        {
            return Task.FromResult(_institutions.Any(institution => institution.Active && institution.Nickname == nickname)); 
        }

        public Task<bool> ExistsByUrlAsync(string url) 
        { 
            return Task.FromResult(_institutions.Any(institution => institution.Active && institution.Url.Equals(url, StringComparison.OrdinalIgnoreCase))); 
        }

        public Task<List<Institution>> GetAllAsync() { return Task.FromResult(_institutions.Where(institution => institution.Active).ToList()); }

        public Task<Institution?> GetByIdAsync(Guid id) { return Task.FromResult(_institutions.SingleOrDefault(institution => institution.Active && institution.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _institutions.RemoveWhere(institution => institution.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
