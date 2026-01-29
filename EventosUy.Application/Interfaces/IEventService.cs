using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IEventService
    {
        public Task<(DTEvent? dt, ValidationResult validation)> CreateAsync(DTInsertEvent dtInsert);
        public Task<(DTEvent? dt, EventCard? card)> GetByIdAsync(Guid id);
        public Task<IEnumerable<EventCard>> GetAllAsync();
        public Task<(DTEvent? dt, ValidationResult validation)> UpdateAsync(DTUpdateEvent dtUpdate);
        public Task<DTEvent?> DeleteAsync(Guid id);
    }
}
