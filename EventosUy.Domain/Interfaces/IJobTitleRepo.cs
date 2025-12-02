using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IJobTitleRepo
    {
        public Task<JobTitle?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<bool> ExistsAsync(string name, Guid institutionId);
        public Task AddAsync(JobTitle jobTitle);
        public Task<bool> RemoveAsync(Guid id);
    }
}
