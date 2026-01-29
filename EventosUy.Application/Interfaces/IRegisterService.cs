using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IRegisterService
    {
        public Task<(DTRegister? dt, ValidationResult validation)> CreateAsync(DTInsertRegisterWithVoucher dtInsert);
        public Task<(DTRegister? dt, ValidationResult validation)> CreateAsync(DTInsertRegisterWithoutVoucher dtInsert);
        public Task<IEnumerable<RegisterCardByEdition>> GetAllByEditionAsync(Guid editionId);
        public Task<IEnumerable<RegisterCardByClient>> GetAllByClientAsync(Guid personId);
    }
}
