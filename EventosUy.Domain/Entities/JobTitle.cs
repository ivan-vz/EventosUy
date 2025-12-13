using EventosUy.Domain.Common;
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

        private JobTitle(string name, string description, Guid institutionId) 
        {
            Id = Guid.NewGuid();    
            Name = name;
            Description = description;
            Created = DateTimeOffset.UtcNow;
            Active = true;
            Institution = institutionId;
        }

        public static Result<JobTitle> Create(string name, string description, Guid institutionId) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name can not be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { errors.Add("Description can not be empty."); }
            if (institutionId == Guid.Empty) { errors.Add("Institution can not be empty."); }

            if (errors.Any()) { return Result<JobTitle>.Failure(errors); }

            JobTitle jobTitleInstance = new JobTitle(name, description, institutionId);

            return Result<JobTitle>.Success(jobTitleInstance);
        }

        public DTJobTitle GetDT(Institution institutionInstance) { return new DTJobTitle(Name, Description, institutionInstance.Nickname, Created); }

        public JobTitleCard GetCard() { return new JobTitleCard(Id, Name); }
    }
}
