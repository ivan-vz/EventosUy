using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepo _repo;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly ISponsorshipService _sponsorshipService;

        public VoucherService(IVoucherRepo voucherRepo, IRegisterTypeService registerTypeService, ISponsorshipService sponsorshipService)
        {
            _repo = voucherRepo;
            _registerTypeService = registerTypeService;
            _sponsorshipService = sponsorshipService;
        }

        public async Task<(DTVoucher? dt, ValidationResult validation)> CreateAsync(DTInsertVoucherWithSponsor dtInsert)
        {
            var validationResult = new ValidationResult();

            var (dtSponsor, sponsorCard) = await _sponsorshipService.GetByIdAsync(dtInsert.Sponsor);
            if (dtSponsor is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Sponsorship", "Sponsorship not found.")
                    );
            }

            if (await _repo.ExistsAsync(dtInsert.Code)) 
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Code", "Code already in use.")
                        );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            int quota = (int)Math.Floor((0.2m * dtSponsor.Amount) / dtSponsor.RegisterType.Price);

            var voucher = new Voucher
                (
                    name: dtInsert.Name,
                    code: dtInsert.Code,
                    discount: dtInsert.Discount,
                    quota: quota,
                    automatic: dtInsert.Automatic,
                    editionId: dtSponsor.Edition.Id,
                    registerTypeId: dtSponsor.RegisterType.Id,
                    sponsorId: dtSponsor.Id
                );

            // TODO: En el apartado de consulta de Sponsor tiene que haber un boton que mande el create al VoucherController con discount = 100 y automatic = false 

            await _repo.AddAsync(voucher);
            
            var dt = new DTVoucher
                (
                    id: voucher.Id,
                    name: voucher.Name,
                    discount: voucher.Discount,
                    quota: voucher.Quota,
                    used: voucher.Used,
                    created: voucher.Created,
                    state: voucher.State,
                    editionCard: dtSponsor.Edition,
                    registerTypeCard: dtSponsor.RegisterType,
                    sponsorCard: sponsorCard
                );
            
            return (dt, validationResult);
        }

        public async Task<(DTVoucher? dt, ValidationResult validation)> CreateAsync(DTInsertVoucherWithoutSponsor dtInsert)
        {
            var validationResult = new ValidationResult();

            var (dtRegisterType, registerTypeCard) = await _registerTypeService.GetByIdAsync(dtInsert.RegisterType);
            if (dtRegisterType is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Register Type", "Register Type not found.")
                    );
            }

            if (await _repo.ExistsAsync(dtInsert.Code))
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Code", "Code already in use.")
                        );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var voucher = new Voucher
                (
                    name: dtInsert.Name,
                    code: dtInsert.Code,
                    discount: dtInsert.Discount,
                    quota: dtRegisterType.Quota,
                    automatic: dtInsert.Automatic,
                    editionId: dtRegisterType.Edition.Id,
                    registerTypeId: dtRegisterType.Id,
                    sponsorId: null
                );

            await _repo.AddAsync(voucher);

            var dt = new DTVoucher
                (
                    id: voucher.Id,
                    name: voucher.Name,
                    discount: voucher.Discount,
                    quota: voucher.Quota,
                    used: voucher.Used,
                    created: voucher.Created,
                    state: voucher.State,
                    editionCard: dtRegisterType.Edition,
                    registerTypeCard: registerTypeCard!,
                    sponsorCard: null
                );

            return (dt, validationResult);
        }

        public async Task<(DTVoucher? dt, VoucherCard? card)> GetByIdAsync(Guid id)
        {
            var voucher = await _repo.GetByIdAsync(id);

            if (voucher is null) { return (null, null); }

            var (dtSponsor, sponsorCard) = voucher.Sponsor is Guid sponsorId ? await _sponsorshipService.GetByIdAsync(sponsorId) : (null, null);
            var (dtRType, rtCard) = await _registerTypeService.GetByIdAsync(voucher.RegisterType);

            var dt = new DTVoucher
                (
                id: voucher.Id,
                name: voucher.Name,
                discount: voucher.Discount,
                quota: voucher.Quota,
                used: voucher.Used,
                created: voucher.Created,
                state: voucher.State,
                editionCard: dtRType.Edition,
                registerTypeCard: rtCard,
                sponsorCard: sponsorCard
                );

            var card = new VoucherCard(Id: voucher.Id, Name: voucher.Name);

            return (dt, card);
        }

        public async Task<(DTVoucher? dt, VoucherCard? card)> GetByCodeAsync(string code)
        {
            var voucher = await _repo.GetByCodeAsync(code);

            if (voucher is null) { return (null, null); }

            var (dtSponsor, sponsorCard) = voucher.Sponsor is Guid sponsorId ? await _sponsorshipService.GetByIdAsync(sponsorId) : (null, null);
            var (dtRType, rtCard) = await _registerTypeService.GetByIdAsync(voucher.RegisterType);

            var dt = new DTVoucher
                (
                id: voucher.Id, 
                name: voucher.Name,
                discount: voucher.Discount,
                quota: voucher.Quota,
                used: voucher.Used,
                created: voucher.Created,
                state: voucher.State,
                editionCard: dtRType.Edition,
                registerTypeCard: rtCard,
                sponsorCard: sponsorCard
                );

            var card = new VoucherCard(Id: voucher.Id, Name: voucher.Name);

            return (dt, card);
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
