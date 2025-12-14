using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class RegisterTypeService : IRegisterTypeService
    {
        private readonly IRegisterTypeRepo _repo;
        private readonly IEditionService _editionService;
        private readonly IEventService _eventService;

        public RegisterTypeService(IRegisterTypeRepo registerTypeRepo, IEditionService editionService, IEventService eventService)
        {
            _repo = registerTypeRepo;
            _editionService = editionService;
            _eventService = eventService;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string description, decimal price, int quota, Guid eventiId)
        {
            List<string> errors = [];
            Result<Event> eventResult = await _eventService.GetByIdAsync(eventiId);
            if (!eventResult.IsSuccess) { errors.AddRange(eventResult.Errors); }

            if (await _repo.ExistsAsync(name)) { errors.Add("Register Type already exist."); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            Result<RegisterType> registerTypeResult = RegisterType.Create(name, description, price, quota, eventiId);
            if (!registerTypeResult.IsSuccess) { return Result<Guid>.Failure(registerTypeResult.Errors); }

            RegisterType registerTypeInstance = registerTypeResult.Value!;
            await _repo.AddAsync(registerTypeInstance);

            return Result<Guid>.Success(registerTypeInstance.Id);
        }

        public async Task<Result<List<RegisterTypeCard>>> GetAllByEditionAsync(Guid editionId)
        {
            if (editionId == Guid.Empty) { return Result<List<RegisterTypeCard>>.Failure("Edition can not be empty."); }
            List<RegisterType> registerTypes = await _repo.GetAllByEditionAsync(editionId);
            List<RegisterTypeCard> cards = registerTypes.Select(registerType => registerType.GetCard()).ToList();

            return Result<List<RegisterTypeCard>>.Success(cards);
        }

        public async Task<Result<RegisterType>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<RegisterType>.Failure("Register Type can not be empty."); }
            RegisterType? registerTypeInstance = await _repo.GetByIdAsync(id);

            if (registerTypeInstance is null) { return Result<RegisterType>.Failure("Register Type not Found."); }

            return Result<RegisterType>.Success(registerTypeInstance);
        }

        public async Task<Result<DTRegisterType>> GetDTAsync(Guid id)
        {
            List<string> errors = [];
            Result<RegisterType> registerTypeResult = await GetByIdAsync(id);
            if (!registerTypeResult.IsSuccess) { errors.AddRange(registerTypeResult.Errors); }
            RegisterType registerTypeInstance = registerTypeResult.Value!;

            Result<Edition> editionResult = await _editionService.GetByIdAsync(registerTypeInstance.Edition);
            if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

            if (errors.Any()) { return Result<DTRegisterType>.Failure(errors); }

            return Result<DTRegisterType>.Success(registerTypeInstance.GetDT(editionResult.Value!));
        }
    }
}
