using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosUy.Infrastructure.Repositories
{
    internal class VoucherRepo : IVoucherRepo
    {
        private readonly ApplicationDbContext _context;

        public VoucherRepo(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task AddAsync(Voucher instance) => await _context.Vouchers.AddAsync(instance);

        public async Task<bool> ExistsAsync(string code) => await _context.Vouchers.AnyAsync(x => x.Code == code);

        public async Task<Voucher?> GetByCodeAsync(string code) => await _context.Vouchers.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<Voucher?> GetByIdAsync(Guid id) => await _context.Vouchers.FirstOrDefaultAsync(x => x.Id == id);

        public void Update(Voucher voucher)
        {
            _context.Vouchers.Attach(voucher);
            _context.Vouchers.Entry(voucher).State = EntityState.Modified;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
