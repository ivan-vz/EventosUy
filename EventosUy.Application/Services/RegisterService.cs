using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class RegisterService : IRegisterService
    {
        private readonly IRegisterRepo _repo;
        private readonly IPersonService _personService;
        private readonly IEditionService _editionService;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly ISponsorshipService _sponsorshipService;
        private readonly IEmploymentService _employmentService;

        public RegisterService(
            IRegisterRepo registerRepo, 
            IPersonService personService, 
            IEditionService editionService, 
            IRegisterTypeService registerTypeService, 
            ISponsorshipService sponsorshipService,
            IEmploymentService employmentService) 
        {
            _repo = registerRepo;
            _personService = personService;
            _editionService = editionService;
            _registerTypeService = registerTypeService;
            _sponsorshipService = sponsorshipService;
            _employmentService = employmentService;

        }

        public async Task<Result<Guid>> CreateAsync(Guid personId, Guid editionId, Guid registerTypeId, Participation participation, string sponsorCode)
        {
            List<string> errors = [];

            Result<Client> personResult = await _personService.GetByIdAsync(personId);
            if (!personResult.IsSuccess) { errors.AddRange(personResult.Errors); }

            Result<Edition> editionResult = await _editionService.GetByIdAsync(editionId);
            if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerTypeId);
            if (!registerTypeResult.IsSuccess) { errors.AddRange(registerTypeResult.Errors); }

            Result<Sponsorship> sponsorResult = await _sponsorshipService.GetByCodeAsync(sponsorCode);
            if (!sponsorResult.IsSuccess) { errors.AddRange(sponsorResult.Errors); }
            
            if (errors.Any()) { return Result<Guid>.Failure(errors); }
            
            RegisterType registerTypeInstance = registerTypeResult.Value!;
            Sponsorship sponsorshipInstance = sponsorResult.Value!;

            if (!sponsorshipInstance.RegisterType.Equals(registerTypeId)) { errors.Add($"Sponsorship {sponsorshipInstance.Name} is not for register type {registerTypeInstance.Name}."); }

            Result contractResult = await _employmentService.HasActiveContractAsync(personId, sponsorshipInstance.Institution);
            if (contractResult.IsFailure) { errors.AddRange(contractResult.Errors); }

            if (participation == Participation.CLIENT || participation == Participation.EMPLOYEE) { errors.Add("Clients and Employees cannot use a sponsor code."); }
           
            if (participation == Participation.GUEST) 
            {
                if (!registerTypeInstance.IsActive()) { errors.Add($"Register Type {registerTypeInstance.Name} is completed."); }

                if (!sponsorshipInstance.IsActive()) { errors.Add($"Sponsorship {sponsorshipInstance.Name} is completed."); }
            }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            if (await _repo.ExistsAsync(personId, editionId)) { return Result<Guid>.Failure("Register already exist."); }

            Result<Register> registerResult = Register.Create(sponsorCode, personId, editionId, registerTypeInstance, participation);
            if (!registerResult.IsSuccess) { return Result<Guid>.Failure(registerResult.Errors); }

            if (participation == Participation.GUEST) 
            {
                Result useSponsorResult = sponsorshipInstance.UseSpot();
                if (useSponsorResult.IsFailure) { return Result<Guid>.Failure(useSponsorResult.Errors); }

                Result useRegisterTypeResult = registerTypeInstance.UseSpot();
                if (useRegisterTypeResult.IsFailure) { return Result<Guid>.Failure(useRegisterTypeResult.Errors); }
            
                //await _sponsorshipService.UpdateAsync(sponsorshipInstance);
                //await _registerTypeService.UpdateAsync(registerTypeInstance);
            }

            Register registerInstance = registerResult.Value!;
            await _repo.AddAsync(registerInstance);

            return Result<Guid>.Success(registerInstance.Id);
        }

        public async Task<Result<Guid>> CreateAsync(Guid personId, Guid editionId, Guid registerTypeId, Participation participation)
        {
            List<string> errors = [];

            Result<Client> personResult = await _personService.GetByIdAsync(personId);
            if (!personResult.IsSuccess) { errors.AddRange(personResult.Errors); }

            Result<Edition> editionResult = await _editionService.GetByIdAsync(editionId);
            if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerTypeId);
            if (!registerTypeResult.IsSuccess) { errors.AddRange(registerTypeResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            RegisterType registerTypeInstance = registerTypeResult.Value!;
            
            if (await _repo.ExistsAsync(personId, editionId)) { return Result<Guid>.Failure("Register already exist."); }

            if (!registerTypeInstance.IsActive()) { return Result<Guid>.Failure($"Register type '{registerTypeInstance.Name}' is full."); }

            Result<Register> registerResult = Register.Create(personId, editionId, registerTypeInstance, participation);
            if (!registerResult.IsSuccess) { return Result<Guid>.Failure(registerResult.Errors); }

            Register registerInstance = registerResult.Value!;

            Result useSpotResult = registerTypeInstance.UseSpot();
            if (!useSpotResult.IsSuccess) { return Result<Guid>.Failure(useSpotResult.Errors); }

            await _repo.AddAsync(registerInstance);
            //await _registerTypeService.UpdateAsync(registerTypeInstance);

            return Result<Guid>.Success(registerInstance.Id);
        }

        public async Task<Result<List<RegisterCardByEdition>>> GetAllByEditionAsync(Guid editionId)
        {
            if (editionId == Guid.Empty) { return Result<List<RegisterCardByEdition>>.Failure("Edition can not be empty."); }
            List<Register> registers = await _repo.GetAllByEditionAsync(editionId);

            List<string> errors = [];
            List<RegisterCardByEdition> cards = [];
            foreach (Register register in registers) 
            {
                Result<Client> personResult = await _personService.GetByIdAsync(register.Person);
                if (!personResult.IsSuccess) { errors.AddRange(personResult.Errors); }

                cards.Add(register.GetCardByEdition(personResult.Value!));
            }

            if (errors.Any()) { return Result<List<RegisterCardByEdition>>.Failure(errors); }

            return Result<List<RegisterCardByEdition>>.Success(cards);
        }

        public async Task<Result<List<RegisterCardByClient>>> GetAllByPersonAsync(Guid personId)
        {
            if (personId == Guid.Empty) { return Result<List<RegisterCardByClient>>.Failure("Person can not be empty."); }
            List<Register> registers = await _repo.GetAllByPersonAsync(personId);

            List<string> errors = [];
            List<RegisterCardByClient> cards = [];
            foreach (Register register in registers)
            {
                Result<Edition> editionResult = await _editionService.GetByIdAsync(register.Edition);
                if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

                cards.Add(register.GetCardByPerson(editionResult.Value!));
            }

            if (errors.Any()) { return Result<List<RegisterCardByClient>>.Failure(errors); }

            return Result<List<RegisterCardByClient>>.Success(cards);
        }

        public async Task<Result<DTRegister>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTRegister>.Failure("Register can not be empty."); }
            Register? registerInstance = await _repo.GetByIdAsync(id);

            if (registerInstance is null) { return Result<DTRegister>.Failure("Register not Found."); }

            List<string> errors = [];
            Result<Edition> editionResult = await _editionService.GetByIdAsync(registerInstance.Edition);
            if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerInstance.RegisterType);
            if (!registerTypeResult.IsSuccess) { errors.AddRange(registerTypeResult.Errors); }

            if (errors.Any()) { return Result<DTRegister>.Failure(errors); }

            return Result<DTRegister>.Success(registerInstance.GetDT(editionResult.Value!, registerTypeResult.Value!));
        }
    }
}
