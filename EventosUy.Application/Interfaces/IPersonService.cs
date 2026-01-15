using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string firstName, string? lastName, string firstSurname, string lastSurname, DateOnly birthday);
        public Task<Result<Client>> GetByIdAsync(Guid personId);
        public Task<Result<List<UserCard>>> GetAllAsync();
        public Task<Result<List<UserCard>>> GetAllExceptAsync(List<Guid> ids);
        public Task<Result<DTClient>> GetDT(Guid id);
    }
}
