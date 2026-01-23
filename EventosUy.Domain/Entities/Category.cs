namespace EventosUy.Domain.Entities
{
    public class Category(string name)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public bool Active { get; set; } = true;
    }
}
