using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IEditionService
    {
        public Task<Result<Guid>> CreateAsync(string name, string initials, Address address, DateOnly from, DateOnly to, Guid eventId, Guid institutionId);
        public Task<Result> ApproveAsync(Guid id);
        public Task<Result> RejectAsync(Guid id);
        public Task<Result<Edition>> GetById(Guid id);
        public Task<Result<List<ActivityCard>>> GetAll();
        public Task<Result<List<ActivityCard>>> GetAllByInstitution(Guid institutionId);
        public Task<Result<List<ActivityCard>>> GetAllByEvent(Guid eventId);
        public Task<Result<List<ActivityCard>>> GetAllPendingByEvent(Guid eventId);
    }
}
