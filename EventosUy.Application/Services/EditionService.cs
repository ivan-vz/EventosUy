using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{
    internal class EditionService : IEditionService
    {
        private readonly IEditionRepo _repo;
        private readonly IEventService _eventService;
        private readonly IInstitutionService _institutionService;

        public EditionService(IEditionRepo editionRepo, IEventService eventService, IInstitutionService institutionService)
        {
            _repo = editionRepo;
            _eventService = eventService;
            _institutionService = institutionService;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string initials, string country, string city, string street, string number, DateOnly from, DateOnly to, Guid eventId, Guid institutionId)
        {
            List<string> errors = [];

            Result<Event> eventResult = await _eventService.GetByIdAsync(eventId);
            if (!eventResult.IsSuccess) { errors.AddRange(eventResult.Errors); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { errors.AddRange(institutionResult.Errors); }

            if (await _repo.ExistsAsync(name)) { errors.Add("Edition already exists."); }

            Result<Address> addressResult = Address.Create(country, city, street, number);
            if (addressResult.IsFailure) { errors.AddRange(addressResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); } 

            Result<Edition> editionResult = Edition.Create(name, initials, from, to, addressResult.Value!, eventId, institutionId);

            if (!editionResult.IsSuccess) { return Result<Guid>.Failure(editionResult.Errors); }

            Edition editionInstance = editionResult.Value!;
            await _repo.AddAsync(editionInstance);

            return Result<Guid>.Success(editionInstance.Id);
        }

        public async Task<Result<List<ActivityCard>>> GetAllAsync()
        {
            List<Edition> editions = await _repo.GetAllAsync();
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<List<ActivityCard>>> GetAllByEventAsync(Guid eventId)
        {
            if (eventId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Event can not be empty."); }
            List<Edition> editions = await _repo.GetAllByEventAsync(eventId);
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<List<ActivityCard>>> GetAllByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Institution can not be empty."); }
            List<Edition> editions = await _repo.GetAllByInstitutionAsync(institutionId);
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<List<ActivityCard>>> GetAllPendingByEventAsync(Guid eventId)
        {
            if (eventId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Event can not be empty."); }
            List<Edition> editions = await _repo.GetAllPendingByEventAsync(eventId);
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<Edition>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Edition>.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _repo.GetByIdAsync(id);
            if (editionInstance is null) { return Result<Edition>.Failure("Edition not Found."); }

            return Result<Edition>.Success(editionInstance);
        }

        public async Task<Result<DTEdition>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTEdition>.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _repo.GetByIdAsync(id);
            if (editionInstance is null) { return Result<DTEdition>.Failure("Edition not Found."); }

            Result<Event> eventResult = await _eventService.GetByIdAsync(editionInstance.Event);
            if (!eventResult.IsSuccess) { return Result<DTEdition>.Failure(eventResult.Errors); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(editionInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTEdition>.Failure(institutionResult.Errors); }

            return Result<DTEdition>.Success(editionInstance.GetDT(eventResult.Value!, institutionResult.Value!));
        }

        public async Task<Result> ApproveAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _repo.GetByIdAsync(id);
            if (editionInstance is null) { return Result.Failure("Edition not Found."); }

            editionInstance.Approve();

            return Result.Success();
        }

        public async Task<Result> RejectAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _repo.GetByIdAsync(id);
            if (editionInstance is null) { return Result.Failure("Edition not Found."); }

            editionInstance.Reject();

            return Result.Success();
        }
    }
}
