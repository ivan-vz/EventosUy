using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IProfessionalProfileRepo
    {
        public Task<ProfessionalProfile?> GetByPersonAsync(Guid personId);
        public Task<List<ProfessionalProfile>> GetAllAsync();
        public Task<List<ProfessionalProfile>> GetAllPendingAsync();
        public Task AddAsync(ProfessionalProfile profile);
        public Task<bool> RemoveAsync(Guid id);
    }
}
