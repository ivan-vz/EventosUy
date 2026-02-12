using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class EditionRepo : IEditionRepo
    {
        private readonly HashSet<Edition> _editions;

        public EditionRepo() { _editions = []; }

        public Task AddAsync(Edition edition) { return Task.FromResult(_editions.Add(edition)); }

        public Task<bool> ExistsByInitialsAsync(string initials) 
        { 
            return Task.FromResult(_editions.Any(edition => edition.State is not EditionState.CANCELLED && edition.Initials.Equals(initials, StringComparison.OrdinalIgnoreCase))); 
        }

        public Task<bool> ExistsByNameAsync(string name) 
        { 
            return Task.FromResult(_editions.Any(edition => edition.State is not EditionState.CANCELLED && edition.Name.Equals(name, StringComparison.OrdinalIgnoreCase))); 
        }

        public Task<bool> ExistsEventAt(string country, string city, string street, string number, int floor, DateOnly from)
        {
            return Task.FromResult( _editions.Any(edition =>
                edition.State is not EditionState.CANCELLED
                && edition.Country.Equals(country, StringComparison.OrdinalIgnoreCase)
                && edition.City.Equals(city, StringComparison.OrdinalIgnoreCase)
                && edition.Street.Equals(street, StringComparison.OrdinalIgnoreCase)
                && edition.Number.Equals(number, StringComparison.OrdinalIgnoreCase)
                && edition.Floor == floor
                && edition.To >= from ) );
        }

        public Task<List<Edition>> GetAllAsync() { return Task.FromResult( _editions.Where(edition => edition.State is EditionState.ONGOING).ToList() ); }

        public Task<List<Edition>> GetAllByEventAsync(Guid eventId) 
        { 
            return Task.FromResult(_editions.Where(edition => edition.State is EditionState.ONGOING && edition.EventId == eventId).ToList()); 
        }

        public Task<List<Edition>> GetAllByInstitutionAsync(Guid institutionId) { return Task.FromResult(_editions.Where(edition => edition.InstitutionId == institutionId).ToList()); }

        public Task<List<Edition>> GetAllPendingByEventAsync(Guid eventId) 
        {
            return Task.FromResult(_editions.Where(edition => edition.State == EditionState.PENDING && edition.EventId == eventId).ToList());
        }

        public Task<Edition?> GetByIdAsync(Guid id) { return Task.FromResult(_editions.SingleOrDefault(edition => edition.Id == id)); }

        public Task<bool> RemoveAsync(Guid id) 
        {
            int result = _editions.RemoveWhere(edition => edition.Id == id);
            return Task.FromResult(result > 0); 
        }
    }
}
