namespace EventosUy.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; private set; }
        public DateTimeOffset Created { get; init; }
        public bool Active { get; private set; }

        public Category(string name, string description) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Created = DateTimeOffset.UtcNow;
            Active = true;
        }
    }
}
