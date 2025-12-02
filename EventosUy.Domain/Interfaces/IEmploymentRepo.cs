using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEmploymentRepo
    {
        public Task<Employment?> GetByIdAsync(Guid id);
        public Task<List<Guid>> GetAllByProfessional(Guid professionalId);
        public Task<List<Guid>> GetAllByInstitutionn(Guid institutionId);
        public Task AddAsync(Employment employment);
        public Task<bool> RemoveAsync(Guid id);
    }
}
