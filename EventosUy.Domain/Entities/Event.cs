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
        public HashSet<Guid> Categories { get; private set; }
        public Guid? Institution { get; init; }

        public Event(string name, string initials, string description, Guid? institution)
        {
            Id = Guid.NewGuid();
            Name = name;
            Initials = initials;
            Description = description;
            Created = DateTimeOffset.UtcNow;
            Active = true;
            Categories = [];
            Institution = institution;
        }

        public void AddCategory(Guid categoryId) { Categories.Add(categoryId); }
        public HashSet<Guid> GetCategories() { return Categories; }
        public DTEvent GetDT(Institution institutionInstance) { return new DTEvent(Name, Initials, Description, institutionInstance.Name, Created); }
        public ActivityCard GetCard() { return new ActivityCard(Id, Name, Initials); }
    }
}
