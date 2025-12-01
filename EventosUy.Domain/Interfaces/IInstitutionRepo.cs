using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IInstitutionRepo
    {
        public Task<Institution> GetByIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public Task AddAsync(Institution institution);
        public Task RemoveAsync(Guid id);
    }
}
