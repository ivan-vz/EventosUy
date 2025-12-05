using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Edition
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Initials { get; init; }
        public DateOnly From { get; private set; }
        public DateOnly To { get; private set; }
        public DateTimeOffset Created { get; init; }
        public Address Address { get; private set; }
        public EditionState State { get; private set; }
        public Guid Event { get; init; }
        public Guid Institution { get; init; }

        private Edition(string name, string initials, DateOnly from, DateOnly to, Address address, Guid eventId, Guid institutionId) 
        {
            Name = name;
            Initials = initials;
            From = from;
            To = to;
            Created = DateTimeOffset.UtcNow;
            Address = address;
            State = EditionState.PENDING;
            Event = eventId;
            Institution = institutionId;
        }

        public static Result<Edition> Create(string name, string initials, DateOnly from, DateOnly to, Address address, Guid eventId, Guid institutionId) 
        {
            if (String.IsNullOrWhiteSpace(name)) { return Result<Edition>.Failure("Name can not be empty."); }
            if (String.IsNullOrWhiteSpace(initials)) { return Result<Edition>.Failure("Initials can not be empty."); }

            if (from > to) { return Result<Edition>.Failure("The start of editing cannot be later than its completion."); }
            if (from <= DateOnly.FromDateTime(DateTime.UtcNow)) { return Result<Edition>.Failure("The start date of the edition cannot be earlier than today's date."); }

            Edition editionInstance = new Edition(name, initials, from, to, address, eventId, institutionId); 

            return Result<Edition>.Success(editionInstance);
        }

        public void Approve() { State = EditionState.PUBLISHED; }
        public void Reject() { State = EditionState.CANCELLED; }

        public DTEdition GetDT(Event eventInstance, Institution institutionInstance) { return new DTEdition(Name, Initials, From, To, Created, Address, eventInstance.Name, institutionInstance.Nickname); }
        public ActivityCard GetCard() { return new ActivityCard(Id, Name, Initials); }
    }
}
