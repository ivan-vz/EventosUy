using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTEdition(string name, string initials, DateOnly from, DateOnly to, DateTimeOffset created, string address, ActivityCard eventCard, UserCard institutioncard)
    {
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public DateOnly From { get; init; } = from;
        public DateOnly To { get; init; } = to;
        public DateTimeOffset Created { get; init; } = created;
        public string FullAddress { get; init; } = address;
        public ActivityCard Event { get; init; } = eventCard;
        public UserCard Institution { get; init; } = institutioncard;
    }
}
