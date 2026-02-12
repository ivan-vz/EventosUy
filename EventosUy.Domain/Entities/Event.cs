namespace EventosUy.Domain.Entities
{
    public class Event(string name, string initials, string description, HashSet<string> categories, Guid institutionId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = name;
        public string Initials { get; set; } = initials;
        public string Description { get; set; } = description;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public bool Active { get; set; } = true;
        public HashSet<string> Categories { get; set; } = categories;
        public Guid InstitutionId { get; init; } = institutionId;
    }
}
