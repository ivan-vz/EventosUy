using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.Interfaces
{
    public interface IVoucherService
    {
        public Task<VoucherCard> CreateAsync(DTInsertVoucher dtInsert);
        public Task<Result<string>> CreateAsync(string code, int discount, bool automatic, Guid editionId);
        public Task<VoucherCard?> GetCardByIdAsync(Guid id);
        public Task<DTVoucher?> GetByCodeAsync(string code);
        public Task UseSpotAsync(Guid id);
    }
}
