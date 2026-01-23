using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Domain.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IClientService
    {
        public Task<(DTClient? Client, ValidationResult ValidationResult)> CreateAsync(DTInsertClient dtInsert);
        public Task<DTClient?> GetByIdAsync(Guid personId);
        public Task<IEnumerable<UserCard>> GetAllAsync();
        public Task<(DTClient? Client, ValidationResult ValidationResult)> UpdateAsync(DTUpdateClient dtUpdate);
        public Task<DTClient?> DeleteAsync(Guid id);
    }
}
