using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class EventService : IEventService
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

        public async Task<(DTEvent? dtEvent, ValidationResult ValidationResult)> CreateAsync(DTInsertEvent dtInsert)
        {
            var validationResult = new ValidationResult();

            if (await _repo.ExistsByNameAsync(dtInsert.Name)) 
            {
                validationResult.Errors.Add
                     (
                         new ValidationFailure("Name", "Name already in use.")
                     );
            }

            if (await _repo.ExistsByInitialsAsync(dtInsert.Initials))
            {
                validationResult.Errors.Add
                     (
                         new ValidationFailure("Initials", "Initials already in use.")
                     );
            }
            
            var categories = dtInsert.Categories.ToHashSet();
            if (!await _categoryService.ExistsAsync(categories)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Category", "Category Not Found.")
                    );
            }

            UserCard? userCard = await _institutionService.GetCardByIdAsync(dtInsert.Institution);
            if (userCard is null) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Institution", "Institution Not Found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var eventInstance = new Event(
                name: dtInsert.Name, 
                initials: dtInsert.Initials, 
                description: dtInsert.Description, 
                institution: dtInsert.Institution,
                categories: categories
                );

            await _repo.AddAsync(eventInstance);

            var dt = new DTEvent(
                    id: eventInstance.Id, 
                    name: eventInstance.Name,
                    initials: eventInstance.Initials,
                    description: eventInstance.Description,
                    created: eventInstance.Created,
                    categories: eventInstance.Categories,
                    card: userCard!
                );

            return (dt, validationResult);
        }

        public async Task<IEnumerable<ActivityCard>> GetAllAsync()
        {
            List<Event> events = await _repo.GetAllAsync();
            List<ActivityCard> cards = [.. events.Select(eventInstance => new ActivityCard(eventInstance.Id, eventInstance.Name, eventInstance.Initials) )];

            return cards;
        }

        public async Task<DTEvent?> GetByIdAsync(Guid id)
        {
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return null; }

            UserCard? institutionCard = await _institutionService.GetCardByIdAsync(eventInstance.Institution);
            
            var dt = new DTEvent(
                    id: eventInstance.Id,
                    name: eventInstance.Name,
                    initials: eventInstance.Initials,
                    description: eventInstance.Description,
                    created: eventInstance.Created,
                    categories: eventInstance.Categories,
                    card: institutionCard!
                );

            return dt;
        }

        public async Task<ActivityCard?> GetCardByIdAsync(Guid id)
        {
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return null; }

            var card = new ActivityCard(eventInstance.Id, eventInstance.Name, eventInstance.Initials);

            return card;
        }

        public async Task<Result<List<ActivityCard>>> GetByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<ActivityCard>>.Failure("Institution can not be empty."); }
            List<Event> events = await _repo.GetAllByInstitutionAsync(institutionId);
            List<ActivityCard> cards = events.Select(eventInstance => eventInstance.GetCard()).ToList();

            return Result<List<ActivityCard>>.Success(cards);
        }

        public async Task<(DTEvent? dtEvent, ValidationResult ValidationResult)> UpdateAsync(DTUpdateEvent dtUpdate)
        {
            var eventInstance = await _repo.GetByIdAsync(dtUpdate.Id);

            var validationResult = new ValidationResult();

            if (eventInstance is null) 
            {
                validationResult.Errors.Add
                     (
                         new ValidationFailure("Id", "Event not found.")
                     );

                return (null, validationResult);
            }

            if (eventInstance.Name != dtUpdate.Name &&  await _repo.ExistsByNameAsync(dtUpdate.Name))
            {
                validationResult.Errors.Add
                     (
                         new ValidationFailure("Name", "Name already in use.")
                     );
            }

            if (eventInstance.Initials != dtUpdate.Initials && await _repo.ExistsByInitialsAsync(dtUpdate.Initials))
            {
                validationResult.Errors.Add
                     (
                         new ValidationFailure("Initials", "Initials already in use.")
                     );
            }

            var categories = dtUpdate.Categories.ToHashSet();
            if (!await _categoryService.ExistsAsync(categories))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Category", "Category Not Found.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }
            
            eventInstance.Name = dtUpdate.Name;
            eventInstance.Initials = dtUpdate.Initials;
            eventInstance.Description = dtUpdate.Description;
            eventInstance.Categories = categories;

            UserCard? institutionCard = await _institutionService.GetCardByIdAsync(eventInstance.Institution);
            
            var dt = new DTEvent(
                    id: eventInstance.Id,
                    name: eventInstance.Name,
                    initials: eventInstance.Initials,
                    description: eventInstance.Description,
                    created: eventInstance.Created,
                    categories: eventInstance.Categories,
                    card: institutionCard!
                );

            return (dt, validationResult);
        }

        public async Task<DTEvent?> DeleteAsync(Guid id)
        {
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return null; }

            eventInstance.Active = false;

            UserCard? institutionCard = await _institutionService.GetCardByIdAsync(eventInstance.Institution);

            var dt = new DTEvent(
                    id: eventInstance.Id,
                    name: eventInstance.Name,
                    initials: eventInstance.Initials,
                    description: eventInstance.Description,
                    created: eventInstance.Created,
                    categories: eventInstance.Categories,
                    card: institutionCard!
                );

            return dt;
        }
    }
}
