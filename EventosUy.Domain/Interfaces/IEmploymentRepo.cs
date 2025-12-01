using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEmploymentRepo
    {
        public Task<Employment> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllByPerson(Guid personId);
        public IEnumerable<Guid> GetAllByInstitutionn(Guid institutionId);
        public IEnumerable<Guid> GetAllByJobTitle(Guid jobTitleId);
        public Task AddAsync(Employment employment);
        public Task RemoveAsync(Guid id);
    }
}
