using EventosUy.Domain.DTOs.Records;
using EventosUy.Application.DTOs.DataTypes.Detail;
using FluentValidation.Results;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;

namespace EventosUy.Application.Interfaces
{
    public interface IEditionService
    {
        public Task<(DTEdition?, ValidationResult)> CreateAsync(DTInsertEdition dtInsert);
        public Task<bool> ApproveAsync(Guid id);
        public Task<bool> RejectAsync(Guid id);
        public Task<(DTEdition?, ValidationResult)> GetByIdAsync(Guid id);
        public Task<IEnumerable<ActivityCard>> GetAllAsync();
        public Task<IEnumerable<ActivityCard>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<IEnumerable<ActivityCard>> GetAllByEventAsync(Guid eventId);
        public Task<IEnumerable<ActivityCard>> GetAllPendingByEventAsync(Guid eventId);
        public Task<(DTEdition?, ValidationResult)> UpdateAsync(DTUpdateEdition dtUpdate);
        public Task<DTEdition?> DeleteAsync(Guid id);
    }
}
