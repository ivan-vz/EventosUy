using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entidades;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using Microsoft.Win32;

namespace EventosUy.Application.Services
{
    internal class RegisterService : IRegisterService
    {
        private readonly IRegisterRepo _repo;
        private readonly IPersonService _personService;
        private readonly IEditionService _editionService;
        private readonly IRegisterTypeService _registerTypeService;
        private readonly ISponsorshipService _sponsorshipService;

        public RegisterService(IRegisterRepo registerRepo, IPersonService personService, IEditionService editionService, IRegisterTypeService registerTypeService, ISponsorshipService sponsorshipService) 
        {
            _repo = registerRepo;
            _personService = personService;
            _editionService = editionService;
            _registerTypeService = registerTypeService;
            _sponsorshipService = sponsorshipService;
        }

        public async Task<Result<Guid>> CreateAsync(Guid personId, Guid editionId, Guid registerTypeId, string sponsorCode, float total, Participation participation)
        {
            Result<Person> personResult = await _personService.GetByIdAsync(personId);
            if (!personResult.IsSuccess) { return Result<Guid>.Failure(personResult.Error!); }

            Result<Edition> editionResult = await _editionService.GetByIdAsync(editionId);
            if (!editionResult.IsSuccess) { return Result<Guid>.Failure(editionResult.Error!); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerTypeId);
            if (!registerTypeResult.IsSuccess) { return Result<Guid>.Failure(registerTypeResult.Error!); }

            if (await _sponsorshipService.ValidateCode(registerTypeId, sponsorCode)) { return Result<Guid>.Failure("Invalid code."); }

            if (await _repo.ExistsAsync(personId, editionId)) { return Result<Guid>.Failure("Register already exist."); }

            Result<Register> registerResult = Register.Create(total, sponsorCode, personId, editionId, registerTypeId, participation);
            if (!registerResult.IsSuccess) { return Result<Guid>.Failure(registerResult.Error!); }

            Register registerInstance = registerResult.Value!;
            await _repo.AddAsync(registerInstance);

            return Result<Guid>.Success(registerInstance.Id);
        }

        public async Task<Result<List<RegisterCardByEdition>>> GetAllByEditionAsync(Guid editionId)
        {
            if (editionId == Guid.Empty) { return Result<List<RegisterCardByEdition>>.Failure("Edition can not be empty."); }
            List<Register> registers = await _repo.GetAllByEditionAsync(editionId);

            List<RegisterCardByEdition> cards = [];
            foreach (Register register in registers) 
            {
                Result<Person> personResult = await _personService.GetByIdAsync(register.Person);
                if (!personResult.IsSuccess) { return Result<List<RegisterCardByEdition>>.Failure(personResult.Error!); }

                cards.Add(register.GetCardByEdition(personResult.Value!));
            }

            return Result<List<RegisterCardByEdition>>.Success(cards);
        }

        public async Task<Result<List<RegisterCardByPerson>>> GetAllByPersonAsync(Guid personId)
        {
            if (personId == Guid.Empty) { return Result<List<RegisterCardByPerson>>.Failure("Person can not be empty."); }
            List<Register> registers = await _repo.GetAllByPersonAsync(personId);

            List<RegisterCardByPerson> cards = [];
            foreach (Register register in registers)
            {
                Result<Edition> editionResult = await _editionService.GetByIdAsync(register.Edition);
                if (!editionResult.IsSuccess) { return Result<List<RegisterCardByPerson>>.Failure(editionResult.Error!); }

                cards.Add(register.GetCardByPerson(editionResult.Value!));
            }

            return Result<List<RegisterCardByPerson>>.Success(cards);
        }

        public async Task<Result<DTRegister>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTRegister>.Failure("Register can not be empty."); }
            Register? registerInstance = await _repo.GetByIdAsync(id);

            if (registerInstance is null) { return Result<DTRegister>.Failure("Register not Found."); }

            Result<Edition> editionResult = await _editionService.GetByIdAsync(registerInstance.Edition);
            if (!editionResult.IsSuccess) { return Result<DTRegister>.Failure(editionResult.Error!); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerInstance.RegisterType);
            if (!registerTypeResult.IsSuccess) { return Result<DTRegister>.Failure(registerTypeResult.Error!); }

            return Result<DTRegister>.Success(registerInstance.GetDT(editionResult.Value!, registerTypeResult.Value!));
        }
    }
}
