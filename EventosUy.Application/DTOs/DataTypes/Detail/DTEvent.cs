using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTEvent(string name, string initials, string description, DateTimeOffset created, IEnumerable<string> categories, UserCard card)
    {
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public string Description { get; init; } = description;
        public DateTimeOffset Created { get; init; } = created;
        public IEnumerable<string> Categories { get; init; } = categories;
        public UserCard Institution { get; init; } = card;
    }
}
