using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IVoucherService
    {
        public Task<(DTVoucher? dt, ValidationResult validation)> CreateAsync(DTInsertVoucherWithSponsor dtInsert);
        public Task<(DTVoucher? dt, ValidationResult validation)> CreateAsync(DTInsertVoucherWithoutSponsor dtInsert);
        public Task<(DTVoucher? dt, VoucherCard? card)> GetByIdAsync(Guid id);
        public Task<(DTVoucher? dt, VoucherCard? card)> GetByCodeAsync(string code);
        public Task UseSpotAsync(Guid id);
    }
}
