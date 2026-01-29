using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IClientService
    {
        public Task<(DTClient? Client, ValidationResult validation)> CreateAsync(DTInsertClient dtInsert);
        public Task<(DTClient? dt, UserCard? card)> GetByIdAsync(Guid id);
        public Task<IEnumerable<UserCard>> GetAllAsync();
        public Task<(DTClient? Client, ValidationResult validation)> UpdateAsync(DTUpdateClient dtUpdate);
        public Task<DTClient?> DeleteAsync(Guid id);
    }
}
