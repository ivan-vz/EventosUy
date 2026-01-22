using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTRegisterType(string name, ActivityCard edition, string description, decimal price, int quota, DateTimeOffset created)
    {
        public string Name { get; init; } = name;
        public ActivityCard Edition { get; init; } = edition;
        public string Description { get; init; } = description;
        public decimal Price { get; init; } = price;
        public int Quota { get; init; } = quota;
        public DateTimeOffset Created { get; init; } = created;
    }
}
