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
        private readonly IEventRepo _eventRepo;
        private readonly ICategoryService _categoryService;
        private readonly IInstitutionService _institutionService;

        public EventService(IEventRepo eventRepo, ICategoryService categoryService, IInstitutionService institutionService) 
        {
            _eventRepo = eventRepo; 
            _categoryService = categoryService;
            _institutionService = institutionService;
        }

        public async Task<Result<Guid>> CreateAsync(string name, string initials, string description, List<Guid> categories, Guid institutionId)
        {
            foreach (var id in categories) 
            {
                Result<Category> categoryResult = await _categoryService.GetByIdAsync(id);
                if (!categoryResult.IsSuccess) { return Result<Guid>.Failure("Categoria invalida"); }
            }

            if (institutionId == Guid.Empty) { return Result<Guid>.Failure("Institution can not be empty."); }
            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { return Result<Guid>.Failure("Institution not Found."); }

            if (await _eventRepo.ExistsAsync(name)) { return Result<Guid>.Failure("Event already exist."); }

            Result<Event> eventResult = Event.Create(name, initials, description, institutionId);

            if (!eventResult.IsSuccess) { return Result<Guid>.Failure(eventResult.Error!); }
            Event eventInstance = eventResult.Value!;
            eventInstance.AddCategories(categories);

            return Result<Guid>.Success(eventInstance.Id);
        }

        public async Task<Result<List<ActivityCard>>> GetAllAsync()
        {
            List<Event> events = await _eventRepo.GetAllAsync();
            List<ActivityCard> cards = events.Select(eventInstance => eventInstance.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<Event>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Event>.Failure("Event can not be empty."); }
            Event? eventInstance = await _eventRepo.GetByIdAsync(id);

            if (eventInstance is null) { return Result<Event>.Failure("Event not Found."); }

            return Result<Event>.Success(eventInstance);
        }

        public async Task<Result<List<ActivityCard>>> GetByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Institution can not be empty."); }
            List<Event> events = await _eventRepo.GetAllByInstitutionAsync(institutionId);
            List<ActivityCard> cards = events.Select(eventInstance => eventInstance.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<Result<DTEvent>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTEvent>.Failure("Event can not be empty."); }
            Event? eventInstance = await _eventRepo.GetByIdAsync(id);

            if (eventInstance is null) { return Result<DTEvent>.Failure("Event not Found."); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(eventInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTEvent>.Failure("Institution not Found."); }

            return Result<DTEvent>.Success(eventInstance.GetDT(institutionResult.Value!));
        }
    }
}
