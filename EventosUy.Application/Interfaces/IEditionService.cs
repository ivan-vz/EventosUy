using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Application.DTOs.DataTypes.Detail;

namespace EventosUy.Application.Interfaces
{
    public interface IEditionService
    {
        public Task<Result<Guid>> CreateAsync(string name, string initials, string country, string city, string street, string number, DateOnly from, DateOnly to, Guid eventId, Guid institutionId);
        public Task<Result> ApproveAsync(Guid id);
        public Task<Result> RejectAsync(Guid id);
        public Task<Result<Edition>> GetByIdAsync(Guid id);
        public Task<Result<DTEdition>> GetDTAsync(Guid id);
        public Task<Result<List<ActivityCard>>> GetAllAsync();
        public Task<Result<List<ActivityCard>>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<Result<List<ActivityCard>>> GetAllByEventAsync(Guid eventId);
        public Task<Result<List<ActivityCard>>> GetAllPendingByEventAsync(Guid eventId);
    }
}
