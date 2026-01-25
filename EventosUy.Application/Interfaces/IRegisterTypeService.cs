using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Domain.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IRegisterTypeService
    {
        public Task<(DTRegisterType?, ValidationResult)> CreateAsync(DTInsertRegisterType dtInsert);
        public Task<DTRegisterType?> GetByIdAsync(Guid id);
        public Task<IEnumerable<RegisterTypeCard>> GetAllByEditionAsync(Guid editionId);
        public Task<DTRegisterType?> DeleteAsync(Guid id);
    }
}
