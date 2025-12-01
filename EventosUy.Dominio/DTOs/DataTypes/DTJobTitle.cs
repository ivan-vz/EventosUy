namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTJobTitle
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Institution { get; init; }
        public DateOnly Created { get; init; }

        public DTJobTitle(string name, string description, string institution, DateOnly created)
        {
            Name = name;
            Description = description;
            Institution = institution;
            Created = created;
        }
    }
}
