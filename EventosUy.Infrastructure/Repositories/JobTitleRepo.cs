using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class JobTitleRepo : IJobTitleRepo
    {
        private readonly HashSet<JobTitle> _jobs;

        public JobTitleRepo() { _jobs = []; }

        public Task AddAsync(JobTitle jobTitle)
        {
            _jobs.Add(jobTitle);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string name, Guid institutionId) { return Task.FromResult(_jobs.Any(job => job.Institution == institutionId && job.Name.Equals(name, StringComparison.OrdinalIgnoreCase))); }

        public Task<List<Guid>> GetAllByInstitutionAsync(Guid institutionId) { return Task.FromResult(_jobs.Where(job => job.Institution == institutionId).Select(job => job.Id).ToList()); }

        public Task<JobTitle?> GetByIdAsync(Guid id) { return Task.FromResult(_jobs.SingleOrDefault(job => job.Id == id)); }

        public Task<bool> RemoveAsync(Guid id)
        {
            int result = _jobs.RemoveWhere(job => job.Id == id);
            return Task.FromResult(result > 0);
        }
    }
}
