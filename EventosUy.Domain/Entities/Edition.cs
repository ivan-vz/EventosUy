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

        public Edition(string name, string initials, DateOnly from, DateOnly to, Address address, Guid eventId, Guid insititutionId) 
        {
            Name = name;
            Initials = initials;
            From = from;
            To = to;
            Created = DateTimeOffset.UtcNow;
            Address = address;
            State = EditionState.PENDING;
            Event = eventId;
            Institution = insititutionId;
        }
    }

}
