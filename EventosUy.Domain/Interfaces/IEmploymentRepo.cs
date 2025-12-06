using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IEmploymentRepo
    {
        public Task<Employment?> GetByIdAsync(Guid id);
        public Task<List<Employment>> GetAllByProfessionalAsync(Guid professionalId);
        public Task<List<Employment>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<bool> ExistsAsync(Guid institutionId, Guid professionalId, Guid jobTitleId);
        public Task AddAsync(Employment employment);
        public Task<bool> RemoveAsync(Guid id);
    }
}
