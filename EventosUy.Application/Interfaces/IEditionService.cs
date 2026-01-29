using EventosUy.Application.DTOs.DataTypes.Detail;
using FluentValidation.Results;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;

namespace EventosUy.Application.Interfaces
{
    public interface IEditionService
    {
        public Task<(DTEdition? dt, ValidationResult validation)> CreateAsync(DTInsertEdition dtInsert);
        public Task<bool> ApproveAsync(Guid id);
        public Task<bool> RejectAsync(Guid id);
        public Task<(DTEdition? dt, EditionCard? card)> GetByIdAsync(Guid id);
        public Task<IEnumerable<EditionCard>> GetAllAsync();
        public Task<IEnumerable<EditionCard>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<IEnumerable<EditionCard>> GetAllByEventAsync(Guid eventId);
        public Task<IEnumerable<EditionCard>> GetAllPendingByEventAsync(Guid eventId);
        public Task<(DTEdition? dt, ValidationResult validation)> UpdateAsync(DTUpdateEdition dtUpdate);
        public Task<DTEdition?> DeleteAsync(Guid id);
    }
}
