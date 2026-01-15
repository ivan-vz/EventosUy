using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Interfaces
{
    public interface IProfessionalProfileService
    {
        public Task<Result<Guid>> RequestVerificationAsync(string linkTree, List<String> specialities, Guid personId);
        public Task<Result<ProfessionalProfile>> GetByIdAsync(Guid professionalId);
        public Task<Result<DTProfessionalProfile>> GetDTAsync(Guid personId);
        public Task<Result<List<UserCard>>> GetAllAsync();
        public Task<Result<List<UserCard>>> GetAllPendingAsync();
        public Task<Result<List<UserCard>>> GetAllUnverifiedAsync();
        public Task<Result> ApproveAsync(Guid personId);
        public Task<Result> RejectAsync(Guid personId);
    }
}
