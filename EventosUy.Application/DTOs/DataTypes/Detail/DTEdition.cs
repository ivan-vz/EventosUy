using EventosUy.Application.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTEdition(
        Guid id, 
        string name, 
        string initials, 
        DateOnly from, 
        DateOnly to, 
        DateTimeOffset created,
        EditionState state,
        string country,
        string city,
        string street,
        string number,
        int floor,
        EventCard eventCard, 
        UserCard institutionCard
        )
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public DateOnly From { get; init; } = from;
        public DateOnly To { get; init; } = to;
        public DateTimeOffset Created { get; init; } = created;
        public EditionState State { get; init; } = state;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public string Number { get; init; } = number;
        public int Floor { get; init; } = floor;
        public EventCard Event { get; init; } = eventCard;
        public UserCard Institution { get; init; } = institutionCard;
    }
}
