using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IJobTitleRepo
    {
        public Task<JobTitle> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByInstitutionAsync(Guid institutionId);
        public Task AddAsync(JobTitle jobTitle);
        public Task RemoveAsync(Guid id);
    }
}
