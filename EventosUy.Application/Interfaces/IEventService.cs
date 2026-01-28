using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IEventService
    {
        public Task<(DTEvent? dtEvent, ValidationResult ValidationResult)> CreateAsync(DTInsertEvent dtInsert);
        public Task<DTEvent?> GetByIdAsync(Guid id);
        public Task<EventCard?> GetCardByIdAsync(Guid id);
        public Task<IEnumerable<EventCard>> GetAllAsync();
        public Task<(DTEvent? dtEvent, ValidationResult ValidationResult)> UpdateAsync(DTUpdateEvent dtUpdate);
        public Task<DTEvent?> DeleteAsync(Guid id);
    }
}
