using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IRegisterTypeService
    {
        public Task<(DTRegisterType? dt, ValidationResult validation)> CreateAsync(DTInsertRegisterType dtInsert);
        public Task<(DTRegisterType? dt, RegisterTypeCard? card)> GetByIdAsync(Guid id);
        public Task<IEnumerable<RegisterTypeCard>> GetAllByEditionAsync(Guid editionId);
        public Task UseSpotAsync(Guid id);
        public Task<DTRegisterType?> DeleteAsync(Guid id);
    }
}
