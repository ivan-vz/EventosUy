namespace EventosUy.Application.DTOs.DataTypes.Update
{
    public class DTUpdateEvent(Guid id, string name, string initials, string description, List<string> categories)
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public string Description { get; init; } = description;
        public List<string> Categories { get; init; } = categories;
    }
}
