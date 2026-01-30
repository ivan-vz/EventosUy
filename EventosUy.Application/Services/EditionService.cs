using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class EditionService : IEditionService
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

        public async Task<(DTEdition? dt, ValidationResult validation)> CreateAsync(DTInsertEdition dtInsert)
        {
            var validationResult = new ValidationResult();

            var (dtEvent, eventCard) = await _eventService.GetByIdAsync(dtInsert.Event);

            if (eventCard is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Event", "Event Not Found.")
                    );
            }

            if (await _repo.ExistsByNameAsync(dtInsert.Name)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Name", "Event already in use.")
                    );
            }

            if (await _repo.ExistsByInitialsAsync(dtInsert.Initials))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Initials", "Initials already in use.")
                    );
            }

            if (await _repo.ExistsEventAt(dtInsert.Country, dtInsert.City, dtInsert.Street, dtInsert.Number, dtInsert.Floor, dtInsert.From))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Address | Date", $"Addres already in booked for {dtInsert.From:dd-MM-yyy}.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            var edition = new Edition
                (
                name: dtInsert.Name, 
                initials: dtInsert.Initials, 
                from: dtInsert.From, 
                to: dtInsert.To, 
                country: dtInsert.Country,
                city: dtInsert.City,
                street: dtInsert.Street,
                number: dtInsert.Number,
                floor: dtInsert.Floor,
                eventId: dtEvent!.Id, 
                institutionId: dtEvent!.Institution.Id
                );

            await _repo.AddAsync(edition);

            var dt = new DTEdition
                (
                id: edition.Id,
                name: dtInsert.Name,
                initials: dtInsert.Initials,
                from: dtInsert.From,
                to: dtInsert.To,
                created: edition.Created,
                state: edition.State,
                country: dtInsert.Country,
                city: dtInsert.City,
                street: dtInsert.Street,
                number: dtInsert.Number,
                floor: dtInsert.Floor,
                eventCard: eventCard!,
                institutionCard: dtEvent.Institution
                );

            return (dt, validationResult);
        }

        public async Task<IEnumerable<EditionCard>> GetAllAsync()
        {
            List<Edition> editions = await _repo.GetAllAsync();
            List<EditionCard> cards = [.. editions.Select(edition => new EditionCard(Id: edition.Id, Name: edition.Name, Initials: edition.Initials, State: edition.State)) ];

            return cards;
        }

        public async Task<IEnumerable<EditionCard>> GetAllByEventAsync(Guid eventId)
        {
            List<Edition> editions = await _repo.GetAllByEventAsync(eventId);
            List<EditionCard> cards = [.. editions.Select(edition => new EditionCard(edition.Id, edition.Name, edition.Initials, State: edition.State) )];

            return cards;
        }

        public async Task<IEnumerable<EditionCard>> GetAllByInstitutionAsync(Guid institutionId)
        {
            List<Edition> editions = await _repo.GetAllByInstitutionAsync(institutionId);
            List<EditionCard> cards = [.. editions.Select(edition => new EditionCard(edition.Id, edition.Name, edition.Initials, State: edition.State))];

            return cards;
        }

        public async Task<IEnumerable<EditionCard>> GetAllPendingByEventAsync(Guid eventId)
        {
            List<Edition> editions = await _repo.GetAllPendingByEventAsync(eventId);
            List<EditionCard> cards = [.. editions.Select(edition => new EditionCard(edition.Id, edition.Name, edition.Initials, State: edition.State))];

            return cards;
        }

        public async Task<(DTEdition? dt, EditionCard? card)> GetByIdAsync(Guid id)
        {
            Edition? edition = await _repo.GetByIdAsync(id);

            if (edition is null || edition.State is not EditionState.ONGOING) { return (null, null); }

            var (dtEvent, eventCard) = await _eventService.GetByIdAsync(edition.Event);

            var dt = new DTEdition
                (
                   id: edition.Id,
                   name: edition.Name,
                   initials: edition.Initials,
                   from: edition.From,
                   to: edition.To,
                   created: edition.Created,
                   state: edition.State,
                   country: edition.Country,
                   city: edition.City,
                   street: edition.Street,
                   number: edition.Number,
                   floor: edition.Floor,
                   eventCard: eventCard!,
                   institutionCard: dtEvent!.Institution
                );

            var card = new EditionCard(edition.Id, edition.Name, edition.Initials, State: edition.State);
            
            return (dt, card);
        }

        public async Task<bool> ApproveAsync(Guid id)
        {
            Edition? edition = await _repo.GetByIdAsync(id);

            if (edition is null || edition.State is not EditionState.PENDING) { return false; }

            edition.State = EditionState.ONGOING;

            return true;
        }

        public async Task<bool> RejectAsync(Guid id)
        {
            Edition? edition = await _repo.GetByIdAsync(id);

            if (edition is null || edition.State is not EditionState.PENDING) { return false; }

            edition.State = EditionState.CANCELLED;

            return true;
        }

        public async Task<(DTEdition? dt, ValidationResult validation)> UpdateAsync(DTUpdateEdition dtUpdate)
        {
            var edition = await _repo.GetByIdAsync(dtUpdate.Id);

            var validationResult = new ValidationResult();

            if (edition is null || edition.State is not EditionState.ONGOING) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Id", "Edition not found.")
                    );

                return (null, validationResult);
            }

            var (dtEvent, eventCard) = await _eventService.GetByIdAsync(edition.Event);
            

            if (!edition.Name.Equals(dtUpdate.Name, StringComparison.OrdinalIgnoreCase) && await _repo.ExistsByNameAsync(dtUpdate.Name))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Name", "Event already in use.")
                    );
            }

            if (!edition.Initials.Equals(dtUpdate.Initials, StringComparison.OrdinalIgnoreCase) && await _repo.ExistsByInitialsAsync(dtUpdate.Initials))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Initials", "Initials already in use.")
                    );
            }

            if (!edition.Country.Equals(dtUpdate.Country, StringComparison.OrdinalIgnoreCase) 
                && !edition.City.Equals(dtUpdate.City, StringComparison.OrdinalIgnoreCase)
                && !edition.Street.Equals(dtUpdate.Street, StringComparison.OrdinalIgnoreCase)
                && !edition.Number.Equals(dtUpdate.Number)
                && edition.Floor != dtUpdate.Floor
                && await _repo.ExistsEventAt(dtUpdate.Country, dtUpdate.City, dtUpdate.Street, dtUpdate.Number, dtUpdate.Floor, dtUpdate.From))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Address | Date", $"Addres already in booked for {dtUpdate.From:dd-MM-yyy}.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }

            edition.Name = dtUpdate.Name;
            edition.Initials = dtUpdate.Initials;
            edition.From = dtUpdate.From;
            edition.To = dtUpdate.To;
            edition.Country = dtUpdate.Country;
            edition.City = dtUpdate.City;
            edition.Street = dtUpdate.Street;
            edition.Number = dtUpdate.Number;
            edition.Floor = dtUpdate.Floor;

            var dt = new DTEdition
                (
                id: edition.Id,
                name: edition.Name,
                initials: edition.Initials,
                from: edition.From,
                to: edition.To,
                created: edition.Created,
                state: edition.State,
                country: edition.Country,
                city: edition.City,
                street: edition.Street,
                number: edition.Number,
                floor: edition.Floor,
                eventCard: eventCard!,
                institutionCard: dtEvent!.Institution
                );

            return (dt, validationResult);
        }

        public async Task<DTEdition?> DeleteAsync(Guid id)
        {
            var edition = await _repo.GetByIdAsync( id );

            if (edition is null || edition.State is EditionState.CANCELLED) { return null; }

            edition.State = EditionState.CANCELLED;

            var (dtEvent, eventCard) = await _eventService.GetByIdAsync(edition.Event);

            var dt = new DTEdition
                (
                id: edition.Id,
                name: edition.Name,
                initials: edition.Initials,
                from: edition.From,
                to: edition.To,
                created: edition.Created,
                state: edition.State,
                country: edition.Country,
                city: edition.City,
                street: edition.Street,
                number: edition.Number,
                floor: edition.Floor,
                eventCard: eventCard!,
                institutionCard: dtEvent!.Institution
                );

            return dt;
        }
    }
}
