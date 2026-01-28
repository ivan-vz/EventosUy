using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface IVoucherService
    {
        public Task<(DTVoucher?, ValidationResult)> CreateAsync(DTInsertVoucherWithSponsor dtInsert);
        public Task<(DTVoucher?, ValidationResult)> CreateAsync(DTInsertVoucherWithoutSponsor dtInsert);
        public Task<DTVoucher?> GetByIdAsync(Guid id);
        public Task<VoucherCard?> GetCardByIdAsync(Guid id);
        public Task<DTVoucher?> GetByCodeAsync(string code);
        public Task UseSpotAsync(Guid id);
    }
}
