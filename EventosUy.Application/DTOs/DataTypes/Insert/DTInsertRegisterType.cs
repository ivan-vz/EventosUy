namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertRegisterType(string name, string description, decimal price, int quota, Guid editionId)
    {
        public string Name { get; init; } = name;
        public string Description { get; init; } = description;
        public decimal Price { get; init; } = price;
        public int Quota { get; init; } = quota;
        public Guid Edition { get; init; } = editionId;
    }
}
