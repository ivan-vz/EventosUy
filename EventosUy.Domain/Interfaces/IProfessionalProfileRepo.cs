using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IProfessionalProfileRepo
    {
        public Task<ProfessionalProfile?> GetByPersonAsync(Guid personId);
        public Task AddAsync(ProfessionalProfile profile);
        public Task<bool> RemoveAsync(Guid id);
    }
}
