namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTCategory(Guid id, string name, DateTimeOffset created)
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public DateTimeOffset Created { get; init; } = created;
    }
}
