using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IEventService
    {
        public Task<Result<Guid>> CreateAsync(string name, string initials, string description, List<Guid> categories, Guid institutionId);
        public Task<Result<Event>> GetByIdAsync(Guid id);
        public Task<Result<List<ActivityCard>>> GetByInstitutionAsync(Guid institutionId);
        public Task<Result<List<ActivityCard>>> GetAllAsync();
        public Task<Result<DTEvent>> GetDTAsync(Guid id);
    }
}
