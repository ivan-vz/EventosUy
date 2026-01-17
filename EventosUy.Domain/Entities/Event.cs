using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Initials { get; init; }
        public string Description { get; private set; }
        public DateTimeOffset Created { get; init; }
        public bool Active { get; private set; }
        public HashSet<string> Categories { get; private set; }
        public Guid Institution { get; init; }

        private Event(string name, string initials, string description, Guid institution)
        {
            Id = Guid.NewGuid();
            Name = name;
            Initials = initials;
            Description = description;
            Created = DateTimeOffset.UtcNow;
            Active = true;
            Institution = institution;
            Categories = [];
        }

        public static Result<Event> Create(string name, string initials, string description, Guid institution) 
        {
            Event eventInstance = new(name, initials, description, institution);
            return Result<Event>.Success(eventInstance);
        }

        public void AddCategories(IEnumerable<string> categories) {
            foreach (var cat in categories)
            {
                Categories.Add(cat);
            }
        }

        public IEnumerable<string> GetCategories() { return Categories; }

        public DTEvent GetDT(Institution institutionInstance) { return new(Name, Initials, Description, Created, Categories, institutionInstance.GetCard()); }

        public ActivityCard GetCard() { return new(Id, Name, Initials); }
    }
}
