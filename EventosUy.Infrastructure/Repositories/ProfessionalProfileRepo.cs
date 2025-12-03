using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class ProfessionalProfileRepo : IProfessionalProfileRepo
    {
        private readonly HashSet<ProfessionalProfile> _professionals;

        public ProfessionalProfileRepo() { _professionals = []; }

        public Task AddAsync(ProfessionalProfile profile)
        {
            _professionals.Add(profile);
            return Task.CompletedTask;
        }

        public Task<ProfessionalProfile?> GetByPersonAsync(Guid personId) { return Task.FromResult(_professionals.SingleOrDefault(professional => professional.Id == personId)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _professionals.RemoveWhere(professional => professional.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
