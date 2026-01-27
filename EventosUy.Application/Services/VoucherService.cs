using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepo _repo;
        private readonly IEditionService _editionService;
        private readonly IRegisterTypeService _registerTypeService;

        public VoucherService(IVoucherRepo voucherRepo, IEditionService editionService, IRegisterTypeService registerTypeService)
        {
            _repo = voucherRepo;
            _editionService = editionService;
            _registerTypeService = registerTypeService;
        }

        public async Task<VoucherCard> CreateAsync(DTInsertVoucher dtInsert)
        {
            var voucher = new Voucher
                (
                    name: dtInsert.Name,
                    code: dtInsert.Code,
                    discount: dtInsert.Discount,
                    quota: dtInsert.quota,
                    automatic: dtInsert.Automatic,
                    editionId: dtInsert.EditionId,
                    registerTypeId: dtInsert.RegisterTypeId,
                    sponsorId: dtInsert.SponsorId
                );

            await _repo.AddAsync( voucher );

            var card = new VoucherCard(voucher.Id, voucher.Name);

            return card;
        }

        public Task<Result<string>> CreateAsync(string code, int discount, bool automatic, Guid editionId)
        {
            throw new NotImplementedException();
        }
        
        public async Task<VoucherCard?> GetCardByIdAsync(Guid id)
        { 
            var voucher = await _repo.GetByIdAsync(id);

            if (voucher is null) { return null;}

            var card = new VoucherCard(Id: voucher.Id, Name: voucher.Name);

            return card;
        }

        public async Task<DTVoucher?> GetByCodeAsync(string code)
        {
            var voucher = await _repo.GetByCodeAsync(code);

            if (voucher is null) { return null; }

            var editionCard = await _editionService.GetCardByIdAsync(voucher.Edition);
            var registerTypeCard = await _registerTypeService.GetCardByIdAsync(voucher.RegisterType);

            var card = new DTVoucher
                (
                id: voucher.Id, 
                name: voucher.Name,
                discount: voucher.Discount,
                quota: voucher.Quota,
                used: voucher.Used,
                created: voucher.Created,
                state: voucher.State,
                editionCard: editionCard!,
                registerTypeCard: registerTypeCard!
                );

            return card;
        }

        public async Task UseSpotAsync(Guid id)
        {
            var voucher = await _repo.GetByIdAsync(id);

            if (voucher is not null)
            {
                voucher.Used++;

                if (voucher.Used == voucher.Quota)
                {
                    voucher.State = VoucherState.COMPLETED;
                }
            }
        }
    }
}
