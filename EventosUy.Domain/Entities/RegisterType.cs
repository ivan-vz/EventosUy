namespace EventosUy.Domain.Entities
{
    public class RegisterType
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Description { get; private set; }
        public float Price { get; init; }
        public int Quota { get; init; }
        public bool Active { get; private set; }
        public DateTimeOffset Created { get; init; }
        public Guid Edition { get; init; }

        public RegisterType(string name, string description, float price, int quota, Guid editionId) 
        {
            Name = name;
            Description = description;
            Price = price;
            Quota = quota;
            Edition = editionId;
            Active = true;
            Created = DateTimeOffset.UtcNow;
        }
    }
}
