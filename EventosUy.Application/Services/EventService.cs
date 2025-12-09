using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class EventService : IEventService
    {
        private readonly IEventRepo _repo;
        private readonly ICategoryService _categoryService;
        private readonly IInstitutionService _institutionService;

        public EventService(IEventRepo eventRepo, ICategoryService categoryService, IInstitutionService institutionService) 
        {
            _repo = eventRepo; 
            _categoryService = categoryService;
            _institutionService = institutionService;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string initials, string description, List<Guid> categories, Guid institutionId)
        {
            List<string> errors = [];
            foreach (var id in categories) 
            {
                Result<Category> categoryResult = await _categoryService.GetByIdAsync(id);
                if (!categoryResult.IsSuccess) { errors.AddRange(categoryResult.Errors); }
            }

            if (institutionId == Guid.Empty) {errors.Add("Institution can not be empty."); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { return Result<Guid>.Failure(institutionResult.Errors); }

            if (await _repo.ExistsAsync(name)) { return Result<Guid>.Failure("Event already exist."); }

            Result<Event> eventResult = Event.Create(name, initials, description, institutionId);

            if (!eventResult.IsSuccess) { return Result<Guid>.Failure(eventResult.Errors); }
            Event eventInstance = eventResult.Value!;
            eventInstance.AddCategories(categories);

            await _repo.AddAsync(eventInstance);

            return Result<Guid>.Success(eventInstance.Id);
        }

        public async Task<Result<List<ActivityCard>>> GetAllAsync()
        {
            List<Event> events = await _repo.GetAllAsync();
            List<ActivityCard> cards = events.Select(eventInstance => eventInstance.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<Event>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Event>.Failure("Event can not be empty."); }
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return Result<Event>.Failure("Event not Found."); }

            return Result<Event>.Success(eventInstance);
        }

        public async Task<Result<List<ActivityCard>>> GetByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Institution can not be empty."); }
            List<Event> events = await _repo.GetAllByInstitutionAsync(institutionId);
            List<ActivityCard> cards = events.Select(eventInstance => eventInstance.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<DTEvent>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTEvent>.Failure("Event can not be empty."); }
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return Result<DTEvent>.Failure("Event not Found."); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(eventInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTEvent>.Failure(institutionResult.Errors); }

            return Result<DTEvent>.Success(eventInstance.GetDT(institutionResult.Value!));
        }
    }
}
