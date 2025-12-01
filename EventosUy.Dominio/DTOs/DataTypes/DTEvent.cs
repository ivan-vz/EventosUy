namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTEvent
    {
        public string Name { get; init; }
        public string Initials { get; init; }
        public string Description { get; init; }
        public string Institution { get; init; }
        public DateOnly Created { get; init; }

        public DTEvent(string name, string initials, string description, string institution, DateOnly created) 
        {
            Name = name;
            Initials = initials;
            Description = description;
            Institution = institution;
            Created = created;
        }
    }
}
