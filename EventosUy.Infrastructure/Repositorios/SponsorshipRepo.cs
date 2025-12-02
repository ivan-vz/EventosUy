using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositorios
{
    internal class SponsorshipRepo : ISponsorshipRepo
    {
        private readonly HashSet<Sponsorship> _sponsorships;

        public SponsorshipRepo() { _sponsorships = []; }

        public Task AddAsync(Sponsorship sponsorship)
        {
            _sponsorships.Add(sponsorship);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid editionId, Guid institutionId) { return Task.FromResult(_sponsorships.Any(sponsor => sponsor.Edition == editionId && sponsor.Institution == institutionId)); }

        public Task<List<Guid>> GetAllByEdition(Guid editionId) { return Task.FromResult(_sponsorships.Where(sponsor => sponsor.Edition == editionId).Select(sponsor => sponsor.Id).ToList()); }

        public Task<List<Guid>> GetAllByInstitution(Guid institutionId) { return Task.FromResult(_sponsorships.Where(sponsor => sponsor.Institution == institutionId).Select(sponsor => sponsor.Id).ToList()); }

        public Task<Sponsorship?> GetByIdAsync(Guid id) { return Task.FromResult(_sponsorships.SingleOrDefault(sponsor => sponsor.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _sponsorships.RemoveWhere(sponsor => sponsor.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
