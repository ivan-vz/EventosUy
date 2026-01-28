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
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepo _repo;
        private readonly IClientService _clientService;
        private readonly IEditionService _editionService;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly ISponsorshipService _sponsorshipService;
        private readonly IVoucherService _voucherService;

        public RegisterService(
            IRegisterRepo registerRepo, 
            IClientService clientService, 
            IEditionService editionService, 
            IRegisterTypeService registerTypeService, 
            ISponsorshipService sponsorshipService,
            IVoucherService voucherService
            )
        {
            _repo = registerRepo;
            _clientService = clientService;
            _editionService = editionService;
            _registerTypeService = registerTypeService;
            _sponsorshipService = sponsorshipService;
            _voucherService = voucherService;
        }

        public async Task<(DTRegister?, ValidationResult)> CreateAsync(DTInsertRegisterWithVoucher dtInsert)
        {
            var validationResult = new ValidationResult();

            var client = await _clientService.GetCardByIdAsync(dtInsert.Client);
            if (client is null) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Client", "Client not found.")
                    );
            }

            var edition = await _editionService.GetCardByIdAsync(dtInsert.Edition);
            if (edition is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition", "Edition not found.")
                    );
            }

            var registerType = await _registerTypeService.GetByIdAsync(dtInsert.RegisterType);
            if (registerType is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Register type", "Register type not found.")
                    );
            }

            var voucher = await _voucherService.GetByCodeAsync(dtInsert.Code);
            if (voucher is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("voucher", "voucher not found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            if (voucher!.Edition.Id != edition!.Id || voucher!.RegisterType.Id != registerType!.Id)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Voucher", "Voucher is not valid.")
                    );
            }

            if (!edition.State.Equals(EditionState.ONGOING)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition state", "Editions is not available.")
                    );
            }

            if (!registerType!.Active) 
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register type state", "Register type is not available.")
                        );
            }

            if (voucher.Used >= voucher.Quota) 
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Voucher Quota", "Voucher is already completed.")
                        );
            }

            if (registerType.Used >= registerType.Quota)
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register Type Quota", "Register Type is already completed.")
                        );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            decimal discount = registerType.Price * voucher.Discount / 100;
            decimal price = registerType.Price - discount;
            
            var register = new Register(
                    total: price,
                    clientId: client!.Id,
                    editionId: edition.Id,
                    registerTypeId: registerType.Id,
                    voucherId: voucher.Id
                );

            await _voucherService.UseSpotAsync(voucher.Id);
            await _registerTypeService.UseSpotAsync(registerType.Id);
            
            await _repo.AddAsync(register);

            var registerTypeCard = await _registerTypeService.GetCardByIdAsync(registerType.Id);
            var voucherCard = await _voucherService.GetCardByIdAsync(voucher.Id);

            var dt = new DTRegister
                (
                    id: register.Id,
                    total: register.Total,
                    created: register.Created,
                    registerTypeCard: registerTypeCard!,
                    editionCard: edition,
                    clientCard: client,
                    voucherCard: voucherCard
                );

            return (dt, validationResult);
        }

        public async Task<(DTRegister?, ValidationResult)> CreateAsync(DTInsertRegisterWithoutVoucher dtInsert)
        {
            var validationResult = new ValidationResult();

            var client = await _clientService.GetCardByIdAsync(dtInsert.Client);
            if (client is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Client", "Client not found.")
                    );
            }

            var edition = await _editionService.GetCardByIdAsync(dtInsert.Edition);
            if (edition is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition", "Edition not found.")
                    );
            }

            var registerType = await _registerTypeService.GetByIdAsync(dtInsert.RegisterType);
            if (registerType is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Register type", "Register type not found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            if (!edition!.State.Equals(EditionState.ONGOING))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition state", "Editions is not available.")
                    );
            }

            if (!registerType!.Active)
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register type state", "Register type is not available.")
                        );
            }

            if (registerType.Used >= registerType.Quota)
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register Type Quota", "Register Type is already completed.")
                        );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var register = new Register(
                    total: registerType.Price,
                    clientId: client!.Id,
                    editionId: edition.Id,
                    registerTypeId: registerType.Id,
                    voucherId: null
                );

            await _registerTypeService.UseSpotAsync(registerType.Id);

            await _repo.AddAsync(register);

            var registerTypeCard = await _registerTypeService.GetCardByIdAsync(registerType.Id);

            var dt = new DTRegister
                (
                    id: register.Id,
                    total: register.Total,
                    created: register.Created,
                    registerTypeCard: registerTypeCard!,
                    editionCard: edition,
                    clientCard: client,
                    voucherCard: null
                );

            return (dt, validationResult);
        }

        public async Task<IEnumerable<RegisterCardByEdition>> GetAllByEditionAsync(Guid id)
        {
            var registers = await _repo.GetAllByEditionAsync(id);

            List<RegisterCardByEdition> cards = [];
            foreach (Register register in registers) 
            {
                var client = await _clientService.GetByIdAsync(register.Client);

                cards.Add( new RegisterCardByEdition(register.Id, client!.Nickname, register.Created) );
            }

            return cards;
        }

        public async Task<IEnumerable<RegisterCardByClient>> GetAllByClientAsync(Guid id)
        {
            var registers = await _repo.GetAllByPersonAsync(id);

            List<RegisterCardByClient> cards = [];
            foreach (Register register in registers)
            {
                var edition = await _editionService.GetByIdAsync(register.Edition);

                cards.Add( new RegisterCardByClient(register.Id, edition!.Name, register.Created) );
            }

            return cards;
        }
    }
}
