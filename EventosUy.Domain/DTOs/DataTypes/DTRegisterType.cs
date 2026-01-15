using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTRegisterType
    {
        public string Name { get; init; }
        public ActivityCard Edition { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int Quota { get; init; }
        public DateTimeOffset Created { get; init; }

        public DTRegisterType(string name, ActivityCard edition, string description, decimal price, int quota, DateTimeOffset created) 
        {
            Name = name;
            Edition = edition;
            Description = description;
            Price = price;
            Quota = quota;
            Created = created;
        }
    }
}
