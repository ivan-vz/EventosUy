using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
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

        public async Task<(DTEvent? dt, ValidationResult validation)> CreateAsync(DTInsertEvent dtInsert)
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

            var userCard = (await _institutionService.GetByIdAsync(dtInsert.Institution)).card;
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
                institutionId: dtInsert.Institution,
                categories: categories
                );

            await _repo.AddAsync(eventInstance);
            await _repo.Save();

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

        public async Task<IEnumerable<EventCard>> GetAllAsync()
        {
            var events = await _repo.GetAllAsync();
            List<EventCard> cards = [.. events.Select(eventInstance => new EventCard(eventInstance.Id, eventInstance.Name, eventInstance.Initials) )];

            return cards;
        }

        public async Task<(DTEvent? dt, EventCard? card)> GetByIdAsync(Guid id)
        {
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return (null, null); }

            var userCard = (await _institutionService.GetByIdAsync(eventInstance.InstitutionId)).card;
            
            var dt = new DTEvent(
                    id: eventInstance.Id,
                    name: eventInstance.Name,
                    initials: eventInstance.Initials,
                    description: eventInstance.Description,
                    created: eventInstance.Created,
                    categories: eventInstance.Categories,
                    card: userCard!
                );

            var card = new EventCard(eventInstance.Id, eventInstance.Name, eventInstance.Initials);

            return (dt, card);
        }

        public async Task<(DTEvent? dt, ValidationResult validation)> UpdateAsync(DTUpdateEvent dtUpdate)
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

            _repo.Update(eventInstance);
            await _repo.Save();

            var userCard = (await _institutionService.GetByIdAsync(eventInstance.InstitutionId)).card;
            
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

        public async Task<DTEvent?> DeleteAsync(Guid id)
        {
            Event? eventInstance = await _repo.GetByIdAsync(id);

            if (eventInstance is null) { return null; }

            eventInstance.Active = false;

            _repo.Update(eventInstance);
            await _repo.Save();

            var userCard = (await _institutionService.GetByIdAsync(eventInstance.InstitutionId)).card;

            var dt = new DTEvent(
                    id: eventInstance.Id,
                    name: eventInstance.Name,
                    initials: eventInstance.Initials,
                    description: eventInstance.Description,
                    created: eventInstance.Created,
                    categories: eventInstance.Categories,
                    card: userCard!
                );

            return dt;
        }
    }
}
