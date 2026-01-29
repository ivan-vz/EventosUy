using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IInstitutionService
    {
        public Task<(DTInstitution? dt, ValidationResult validation)> CreateAsync(DTInsertInstitution dtInser);
        public Task<(DTInstitution? dt, UserCard? card)> GetByIdAsync(Guid id);
        public Task<IEnumerable<UserCard>> GetAllAsync();
        public Task<(DTInstitution? dt, ValidationResult validation)> UpdateAsync(DTUpdateInstitution dtUpdate);
        public Task<DTInstitution?> DeleteAsync(Guid id);
    }
}
