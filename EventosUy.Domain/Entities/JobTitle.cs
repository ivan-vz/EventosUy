using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.Entities
{
    public class JobTitle
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; private set; }
        public DateTimeOffset Created { get; init; }
        public bool Active { get; private set; }
        public Guid Institution { get; init; }

        public JobTitle(string name, string description, Guid institutionId) 
        {
            Name = name;
            Description = description;
            Created = DateTimeOffset.UtcNow;
            Active = true;
            Institution = institutionId;
        }

        public DTJobTitle GetDT(Institution institutionInstance) { return new DTJobTitle(Name, Description, institutionInstance.Nickname, Created); }

        public JobTitleCard GetJobTitleCard() { return new JobTitleCard(Id, Name); }
    }
}
