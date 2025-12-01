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

        public void AgregarCategoria(Guid id_categoria) { Categories.Add(id_categoria); }
    }
}
