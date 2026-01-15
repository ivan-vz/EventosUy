using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTEdition
    {
        public string Name { get; init; }
        public string Initials { get; init; }
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public DateTimeOffset Created { get; init; }
        public string FullAddress { get; init; }
        public ActivityCard Event { get; init; }
        public UserCard Institution { get; init; }

        public DTEdition(string name, string initials, DateOnly from, DateOnly to, DateTimeOffset created, string address, ActivityCard eventCard, UserCard institutioncard)
        {
            Name = name;
            Initials = initials;
            From = from;
            To = to;
            Created = created;
            FullAddress = address;
            Event = eventCard;
            Institution = institutioncard;
        }
    }
}
