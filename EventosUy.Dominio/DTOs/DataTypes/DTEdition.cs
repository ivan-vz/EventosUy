using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTEdition
    {
        public string Name { get; init; }
        public string Initials { get; init; }
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public DateOnly Created { get; init; }
        public Address Address { get; init; }
        public string Event { get; init; }
        public string Institution { get; init; }

        public DTEdition(string name, string initials, DateOnly from, DateOnly to, DateOnly created, Address address, string eventName, string institution)
        {
            Name = name;
            Initials = initials;
            From = from;
            To = to;
            Created = created;
            Address = address;
            Event = eventName;
            Institution = institution;
        }

    }
}
