using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTRegisterType(Guid id, string name, string description, decimal price, int quota, DateTimeOffset created, ActivityCard editionCard)
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public string Description { get; init; } = description;
        public decimal Price { get; init; } = price;
        public int Quota { get; init; } = quota;
        public DateTimeOffset Created { get; init; } = created;
        public ActivityCard Edition { get; init; } = editionCard;
    }
}
