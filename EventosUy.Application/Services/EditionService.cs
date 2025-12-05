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
        private readonly IEditionRepo _editionRepo;
        private readonly IEventService _eventService;
        private readonly IInstitutionService _institutionService;

        public EditionService(IEditionRepo editionRepo, IEventService eventService, IInstitutionService institutionService)
        {
            _editionRepo = editionRepo;
            _eventService = eventService;
            _institutionService = institutionService;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string initials, Address address, DateOnly from, DateOnly to, Guid eventId, Guid institutionId)
        {
            Result<Event> eventResult = await _eventService.GetByIdAsync(eventId);
            if (!eventResult.IsSuccess) { return Result<Guid>.Failure(eventResult.Error!); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { return Result<Guid>.Failure(institutionResult.Error!); }

            if (await _editionRepo.ExistsAsync(name)) { return Result<Guid>.Failure("Edition already exists."); }

            Result<Edition> editionResult = Edition.Create(name, initials, from, to, address, eventId, institutionId);

            if (!editionResult.IsSuccess) { return Result<Guid>.Failure(editionResult.Error!); }

            Edition editionInstance = editionResult.Value!;
            await _editionRepo.AddAsync(editionInstance!);

            return Result<Guid>.Success(editionInstance!.Id);
        }

        public async Task<Result<List<ActivityCard>>> GetAllAsync()
        {
            List<Edition> editions = await _editionRepo.GetAllAsync();
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<List<ActivityCard>>> GetAllByEventAsync(Guid eventId)
        {
            if (eventId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Event can not be empty."); }
            List<Edition> editions = await _editionRepo.GetAllByEventAsync(eventId);
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<List<ActivityCard>>> GetAllByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Institution can not be empty."); }
            List<Edition> editions = await _editionRepo.GetAllByInstitutionAsync(institutionId);
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<List<ActivityCard>>> GetAllPendingByEventAsync(Guid eventId)
        {
            if (eventId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Event can not be empty."); }
            List<Edition> editions = await _editionRepo.GetAllPendingByEventAsync(eventId);
            List<ActivityCard> cards = editions.Select(edition => edition.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<Edition>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Edition>.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _editionRepo.GetByIdAsync(id);
            if (editionInstance is null) { return Result<Edition>.Failure("Edition not Found."); }

            return Result<Edition>.Success(editionInstance);
        }

        public async Task<Result<DTEdition>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTEdition>.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _editionRepo.GetByIdAsync(id);
            if (editionInstance is null) { return Result<DTEdition>.Failure("Edition not Found."); }

            Result<Event> eventResult = await _eventService.GetByIdAsync(editionInstance.Event);
            if (!eventResult.IsSuccess) { return Result<DTEdition>.Failure(eventResult.Error!); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(editionInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTEdition>.Failure(institutionResult.Error!); }

            return Result<DTEdition>.Success(editionInstance.GetDT(eventResult.Value!, institutionResult.Value!));
        }

        public async Task<Result> ApproveAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _editionRepo.GetByIdAsync(id);
            if (editionInstance is null) { return Result.Failure("Edition not Found."); }

            editionInstance.Approve();

            return Result.Success();
        }

        public async Task<Result> RejectAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result.Failure("Edition can not be empty."); }
            Edition? editionInstance = await _editionRepo.GetByIdAsync(id);
            if (editionInstance is null) { return Result.Failure("Edition not Found."); }

            editionInstance.Reject();

            return Result.Success();
        }
    }
}
