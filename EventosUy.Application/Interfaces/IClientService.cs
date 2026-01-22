using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.Interfaces
{
    public interface IClientService
    {
        public Task<Result<Guid>> CreateAsync(string nickname, string password, string email, string firstName, string? lastName, string firstSurname, string lastSurname, DateOnly birthday);
        public Task<DTClient?> GetByIdAsync(Guid personId);
        public Task<IEnumerable<UserCard>> GetAllAsync();
        public Task<Result<DTClient>> GetDT(Guid id);
    }
}
