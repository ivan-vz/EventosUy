using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using System.Runtime.InteropServices;

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
        public HashSet<Guid> Categories { get; private set; }
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
            Categories = new HashSet<Guid>();
        }

        public static Result<Event> Create(string name, string initials, string description, Guid institution) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name cannot be empty."); }
            if (string.IsNullOrWhiteSpace(initials)) { errors.Add("Initials cannot be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { errors.Add("Description cannot be empty."); }
            if (institution == Guid.Empty) { errors.Add("Institution cannot be empty."); }

            if (errors.Any()) { return Result<Event>.Failure(errors); }

            Event eventInstance = new Event(name, initials, description, institution);

            return Result<Event>.Success(eventInstance);
        }

        public void AddCategories(IEnumerable<Guid> categories) {
            foreach (var id in categories)
            {
                Categories.Add(id);
            }
        }

        public HashSet<Guid> GetCategories() { return Categories; }

        public DTEvent GetDT(Institution institutionInstance) { return new DTEvent(Name, Initials, Description, institutionInstance.Name, Created); }

        public ActivityCard GetCard() { return new ActivityCard(Id, Name, Initials); }
    }
}
