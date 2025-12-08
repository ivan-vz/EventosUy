using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string firstName, string? lastName, string firstSurname, string lastSurname, DateOnly birthday);
        public Task<Result<Person>> GetByIdAsync(Guid personId);
        public Task<Result<List<ProfileCard>>> GetAllAsync();
        public Task<Result<List<ProfileCard>>> GetAllExceptAsync(List<Guid> ids);
        public Task<Result<DTPerson>> GetDT(Guid id);
    }
}
