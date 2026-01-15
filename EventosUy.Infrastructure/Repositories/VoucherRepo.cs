using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Infrastructure.Repositories
{
    internal class VoucherRepo : IVoucherRepo
    {
        private readonly HashSet<Voucher> _vouchers;

        public VoucherRepo() { _vouchers = []; }

        public Task AddAsync(Voucher instance)
        {
            _vouchers.Add(instance);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string code) { return Task.FromResult(_vouchers.Any(voucher => voucher.IsActive == true && voucher.Code.Equals(code))); }

        public Task<Voucher?> GetByCodeAsync(string code) { return Task.FromResult(_vouchers.SingleOrDefault(voucher => voucher.IsActive == true && voucher.Code.Equals(code))); }

        public Task<bool> RemoveAsync(string code)
        {
            int result = _vouchers.RemoveWhere(voucher => voucher.Code == code);
            return Task.FromResult(result > 0);
        }
    }
}
