using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTEvent
    {
        public string Name { get; init; }
        public string Initials { get; init; }
        public string Description { get; init; }
        public DateTimeOffset Created { get; init; }
        public IEnumerable<string> Categories { get; init; }
        public UserCard Institution { get; init; }

        public DTEvent(string name, string initials, string description, DateTimeOffset created, IEnumerable<string> categories, UserCard card) 
        {
            Name = name;
            Initials = initials;
            Description = description;
            Created = created;
            Categories = categories;
            Institution = card;
        }
    }
}
