namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertEvent(string name, string initials, string description, List<string> categories, Guid institution)
    {
        public string Name { get; init; } = name;
        public string Initials { get; init; } = initials;
        public string Description { get; init; } = description;
        public List<string> Categories { get; init; } = categories;
        public Guid Institution { get; init; } = institution;
    }
}
