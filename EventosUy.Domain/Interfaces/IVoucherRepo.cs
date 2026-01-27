using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Interfaces
{
    public interface IVoucherRepo
    {
        public Task<Voucher?> GetByCodeAsync(string code);
        public Task<Voucher?> GetByIdAsync(Guid id);
        public Task<bool> ExistsAsync(string code);
        public Task AddAsync(Voucher instance);
        public Task<bool> RemoveAsync(string code);
    }
}
