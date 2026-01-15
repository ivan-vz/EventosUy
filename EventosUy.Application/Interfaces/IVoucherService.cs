using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IVoucherService
    {
        public Task<Result<string>> CreateAsync(int discount, bool automatic, Guid editionId);
        public Task<Result<string>> CreateAsync(string code, int discount, bool automatic, Guid editionId);
        public Task<Result<Voucher>> GetByCode(string code);
    }
}
