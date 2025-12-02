using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositorios
{
    internal class EditionRepo : IEditionRepo
    {
        private HashSet<Edition> _editions;

        public EditionRepo() { _editions = []; }

        public Task AddAsync(Edition edition)
        {
            _editions.Add(edition);
            return Task.CompletedTask;
        }
        
        public Task<bool> AlreadyExists(string name) { return Task.FromResult(_editions.Any(edition => edition.Name.Equals(name, StringComparison.OrdinalIgnoreCase))); }

        public Task<List<Guid>> GetAllAsync() { return Task.FromResult( _editions.Select(edition => edition.Id).ToList() ); }

        public Task<List<Guid>> GetAllByEventAsync(Guid eventId) { return Task.FromResult(_editions.Where(edition => edition.Event == eventId).Select(edition => edition.Id).ToList()); }

        public Task<List<Guid>> GetAllByInstitutionAsync(Guid institutionId) { return Task.FromResult(_editions.Where(edition => edition.Institution == institutionId).Select(edition => edition.Id).ToList()); }

        public Task<List<Guid>> GetAllPendingByEvent(Guid eventId) 
        {
            return Task.FromResult(_editions.Where(edition => edition.Event == eventId && edition.State == EditionState.PENDING).Select(edition => edition.Id).ToList());
        }

        public Task<Edition?> GetByIdAsync(Guid id) { return Task.FromResult(_editions.SingleOrDefault(edition => edition.Id == id)); }

        public Task RemoveAsync(Guid id)
        {
            _editions.RemoveWhere(edition => edition.Id == id);
            return Task.CompletedTask;
        }
    }
}
