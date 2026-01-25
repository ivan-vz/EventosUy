namespace EventosUy.Application.DTOs.DataTypes.Update
{
    public class DTUpdateEdition(
        Guid id,
        string name,
        string initials,
        DateOnly from,
        DateOnly to,
        string country,
        string city,
        string street,
        string number,
        int floor
        )
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public DateOnly From { get; init; } = from;
        public DateOnly To { get; init; } = to;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public string Number { get; init; } = number;
        public int Floor { get; init; } = floor;
    }
}
