using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertEdition(
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
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public DateOnly From { get; init; } = from;
        public DateOnly To { get; init; } = to;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public string Number { get; init; } = number;
        public int Floor { get; init; } = floor;
        public Guid Event { get; init; } = eventId;
        public Guid Institution { get; init; } = institutionId;
    }
}
