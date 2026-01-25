using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IEventService
    {
        public Task<(DTEvent? dtEvent, ValidationResult ValidationResult)> CreateAsync(DTInsertEvent dtInsert);
        public Task<DTEvent?> GetByIdAsync(Guid id);
        public Task<ActivityCard?> GetCardByIdAsync(Guid id);
        public Task<Result<List<ActivityCard>>> GetByInstitutionAsync(Guid institutionId);
        public Task<IEnumerable<ActivityCard>> GetAllAsync();
        public Task<(DTEvent? dtEvent, ValidationResult ValidationResult)> UpdateAsync(DTUpdateEvent dtUpdate);
        public Task<DTEvent?> DeleteAsync(Guid id);
    }
}
