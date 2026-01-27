using EventosUy.Application.DTOs.Records;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTRegisterType(Guid id, string name, string description, decimal price, int quota, int used, DateTimeOffset created, bool active, EditionCard editionCard)
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public string Description { get; init; } = description;
        public decimal Price { get; init; } = price;
        public int Quota { get; init; } = quota;
        public int Used { get; init; } = used;
        public DateTimeOffset Created { get; init; } = created;
        public bool Active { get; init; } = active;
        public EditionCard Edition { get; init; } = editionCard;
    }
}
