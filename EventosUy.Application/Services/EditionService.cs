using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
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

        public Task<Result> ApproveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Guid>> CreateAsync(string name, string initials, Address address, DateOnly from, DateOnly to, Guid eventId, Guid institutionId)
        {
            if (String.IsNullOrWhiteSpace(name)) { return Result<Guid>.Failure("Name can not be empty."); }
            if (String.IsNullOrWhiteSpace(initials)) { return Result<Guid>.Failure("Initials can not be empty."); }

            if (from > to) { return Result<Guid>.Failure("The start of editing cannot be later than its completion."); }
            if (from <= DateOnly.FromDateTime(DateTime.UtcNow)) { return Result<Guid>.Failure("The start date of the edition cannot be earlier than today's date."); }

            if (eventId == Guid.Empty) { return Result<Guid>.Failure("Event can not be empty."); }
            Event eventInstance = await _eventService.GetByIdAsync(eventId);
            if (eventInstance == null) { return Result<Guid>.Failure("The event does not exist."); }

            if (institutionId == Guid.Empty) { return Result<Guid>.Failure("Institution can not be empty."); }
            Institution institutionInstance = _institutionService.GetByIdAsync(institutionId);
            if (institutionInstance == null) { return Result<Guid>.Failure("The institution does not exist."); }

            if (await _editionRepo.ExistsAsync(name)) { return Result<Guid>.Failure("Edition already exists."); }

            Edition edition = new Edition(name, initials, from, to, address, eventId, institutionId);
            await _editionRepo.AddAsync(edition);

            return Result<Guid>.Success(edition.Id);
        }

        public Task<Result<List<ActivityCard>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ActivityCard>>> GetAllByEvent(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ActivityCard>>> GetAllByInstitution(Guid institutionId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ActivityCard>>> GetAllPendingByEvent(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Edition>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RejectAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
