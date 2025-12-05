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
        
        public Task<bool> ExistsAsync(string name) { return Task.FromResult(_editions.Any(edition => edition.Name.Equals(name, StringComparison.OrdinalIgnoreCase))); }

        public Task<List<Edition>> GetAllAsync() { return Task.FromResult( _editions.ToList() ); }

        public Task<List<Edition>> GetAllByEventAsync(Guid eventId) { return Task.FromResult(_editions.Where(edition => edition.Event == eventId).ToList()); }

        public Task<List<Edition>> GetAllByInstitutionAsync(Guid institutionId) { return Task.FromResult(_editions.Where(edition => edition.Institution == institutionId).ToList()); }

        public Task<List<Edition>> GetAllPendingByEventAsync(Guid eventId) 
        {
            return Task.FromResult(_editions.Where(edition => edition.Event == eventId && edition.State == EditionState.PENDING).ToList());
        }

        public Task<Edition?> GetByIdAsync(Guid id) { return Task.FromResult(_editions.SingleOrDefault(edition => edition.Id == id)); }

        public Task<bool> RemoveAsync(Guid id) 
        {
            int result = _editions.RemoveWhere(edition => edition.Id == id);
            return Task.FromResult(result > 0); 
        }
    }
}
