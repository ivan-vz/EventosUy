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
        private readonly IVoucherService _voucherService;

        public RegisterService(
            IRegisterRepo registerRepo, 
            IClientService clientService, 
            IEditionService editionService, 
            IRegisterTypeService registerTypeService,
            IVoucherService voucherService
            )
        {
            _repo = registerRepo;
            _clientService = clientService;
            _editionService = editionService;
            _registerTypeService = registerTypeService;
            _voucherService = voucherService;
        }

        public async Task<(DTRegister? dt, ValidationResult validation)> CreateAsync(DTInsertRegisterWithVoucher dtInsert)
        {
            var validationResult = new ValidationResult();

            var userCard = (await _clientService.GetByIdAsync(dtInsert.Client)).card;
            if (userCard is null) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Client", "Client not found.")
                    );
            }

            var (dtRegisterType, registerTypeCard) = await _registerTypeService.GetByIdAsync(dtInsert.RegisterType);
            if (dtRegisterType is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Register type", "Register type not found.")
                    );
            }

            var (dtVoucher, voucherCard) = await _voucherService.GetByCodeAsync(dtInsert.Code);
            if (dtVoucher is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Voucher", "voucher not found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            if (dtVoucher!.Edition.Id != dtRegisterType!.Edition.Id || dtVoucher!.RegisterType.Id != dtRegisterType!.Id)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Voucher", "Voucher is not valid.")
                    );
            }

            if (!dtRegisterType.Edition.State.Equals(EditionState.ONGOING)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition state", "Editions is not available.")
                    );
            }

            if (!dtRegisterType!.Active) 
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register type state", "Register type is not available.")
                        );
            }

            if (dtVoucher.Used >= dtVoucher.Quota) 
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Voucher Quota", "Voucher is already completed.")
                        );
            }

            if (dtRegisterType.Used >= dtRegisterType.Quota)
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register Type Quota", "Register Type is already completed.")
                        );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            decimal discount = dtRegisterType.Price * dtVoucher.Discount / 100;
            decimal price = dtRegisterType.Price - discount;
            
            var register = new Register(
                    total: price,
                    clientId: userCard!.Id,
                    editionId: dtRegisterType.Edition.Id,
                    registerTypeId: registerTypeCard!.Id,
                    voucherId: voucherCard!.Id
                );

            await _voucherService.UseSpotAsync(voucherCard.Id);
            await _registerTypeService.UseSpotAsync(registerTypeCard.Id);
            
            await _repo.AddAsync(register);

            var dt = new DTRegister
                (
                    id: register.Id,
                    total: register.Total,
                    created: register.Created,
                    registerTypeCard: registerTypeCard!,
                    editionCard: dtRegisterType.Edition,
                    clientCard: userCard,
                    voucherCard: voucherCard
                );

            return (dt, validationResult);
        }

        public async Task<(DTRegister? dt, ValidationResult validation)> CreateAsync(DTInsertRegisterWithoutVoucher dtInsert)
        {
            var validationResult = new ValidationResult();

            var userCard = (await _clientService.GetByIdAsync(dtInsert.Client)).card;
            if (userCard is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Client", "Client not found.")
                    );
            }

            var (dtRegisterType, registerTypeCard) = await _registerTypeService.GetByIdAsync(dtInsert.RegisterType);
            if (dtRegisterType is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Register type", "Register type not found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            if (!dtRegisterType!.Edition.State.Equals(EditionState.ONGOING))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition state", "Editions is not available.")
                    );
            }

            if (!dtRegisterType!.Active)
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register type state", "Register type is not available.")
                        );
            }

            if (dtRegisterType.Used >= dtRegisterType.Quota)
            {
                validationResult.Errors.Add
                        (
                            new ValidationFailure("Register Type Quota", "Register Type is already completed.")
                        );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var register = new Register(
                    total: dtRegisterType.Price,
                    clientId: userCard!.Id,
                    editionId: dtRegisterType!.Edition.Id,
                    registerTypeId: registerTypeCard!.Id,
                    voucherId: null
                );

            await _registerTypeService.UseSpotAsync(dtRegisterType.Id);

            await _repo.AddAsync(register);

            var dt = new DTRegister
                (
                    id: register.Id,
                    total: register.Total,
                    created: register.Created,
                    registerTypeCard: registerTypeCard!,
                    editionCard: dtRegisterType.Edition,
                    clientCard: userCard,
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
                var card = (await _clientService.GetByIdAsync(register.ClientId)).card;

                cards.Add( new RegisterCardByEdition(register.Id, card!.Nickname, register.Created) );
            }

            return cards;
        }

        public async Task<IEnumerable<RegisterCardByClient>> GetAllByClientAsync(Guid id)
        {
            var registers = await _repo.GetAllByClientAsync(id);

            List<RegisterCardByClient> cards = [];
            foreach (Register register in registers)
            {
                var card = (await _editionService.GetByIdAsync(register.EditionId)).card;

                cards.Add( new RegisterCardByClient(register.Id, card!.Name, register.Created) );
            }

            return cards;
        }
    }
}
