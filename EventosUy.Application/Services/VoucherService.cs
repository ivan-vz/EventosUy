using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class VoucherService : IVoucherService
    {
        private readonly IVoucherRepo _voucherRepo;
        private readonly IEditionService _EditionService;

        public VoucherService(IVoucherRepo voucherRepo, IEditionService editionService)
        {
            _voucherRepo = voucherRepo;
            _EditionService = editionService;
        }

        public async Task<Result<string>> CreateAsync(int discount, bool automatic, Guid editionId)
        {
            if (editionId == Guid.Empty) { return Result<string>.Failure("Edition cannot be empty."); }
            Result<Edition> editionResult = await _EditionService.GetByIdAsync(editionId);
            if (editionResult.IsFailure) { return Result<string>.Failure("Edition not Found."); }



            throw new NotImplementedException();
        }

        public Task<Result<string>> CreateAsync(string code, int discount, bool automatic, Guid editionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Voucher>> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) { return Result<Voucher>.Failure("Code cannot be empty."); }

            Voucher? instance = await _voucherRepo.GetByCodeAsync(code);
            if (instance is null) { return Result<Voucher>.Failure("Voucher not Found.");}

            return Result<Voucher>.Success(instance);
        }
    }
}
