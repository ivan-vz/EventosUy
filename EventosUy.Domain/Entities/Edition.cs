using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Edition(
        string name, 
        string initials, 
        DateOnly from, 
        DateOnly to,
        string country,
        string city,
        string street,
        string number,
        int floor,
        Guid eventId, 
        Guid institutionId
        )
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = name;
        public string Initials { get; set; } = initials;
        public DateOnly From { get; set; } = from;
        public DateOnly To { get; set; } = to;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public string Country { get; set; } = country;
        public string City { get; set; } = city;
        public string Street { get; set; } = street;
        public string Number { get; set; } = number;
        public int Floor { get; set; } = floor;
        public EditionState State { get; set; } = EditionState.PENDING;
        public Guid EventId { get; init; } = eventId;
        public Guid InstitutionId { get; init; } = institutionId;
    }
}
