using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Employment
    {
        public Guid Id { get; init; }
        public DateTimeOffset Created { get; init; }
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public EmploymentState State { get; set; }
        public Guid JobTitle { get; set; }
        public Guid ProfessionalProfile { get; init; }
        public Guid Institution { get; init; }

        private Employment(DateOnly from, DateOnly to, Guid jobTitleId, Guid professionalId, Guid institutionId) 
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.UtcNow;
            From = from;
            To = to;
            State = EmploymentState.ACTIVE;
            JobTitle = jobTitleId;
            ProfessionalProfile = professionalId;
            Institution = institutionId;
        }

        public static Result<Employment> Create(DateOnly from, DateOnly to, Guid jobTitleId, Guid professionalId, Guid institutionId) 
        {
            List<string> errors = [];
            if (from > to) { errors.Add("Starting date cannot be after ending's date"); }
            if (from < DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Starting date cannot be before todays's date"); }

            if (jobTitleId == Guid.Empty) { errors.Add("JobTitle cannot me empty."); }
            if (professionalId == Guid.Empty) { errors.Add("Professional profile cannot me empty."); }
            if (institutionId == Guid.Empty) { errors.Add("Institution cannot me empty."); }

            if (errors.Any()) { return Result<Employment>.Failure(errors); }

            Employment employmentInstance = new Employment(from, to, jobTitleId, professionalId, institutionId);

            return Result<Employment>.Success(employmentInstance);
        }

        public DTEmployment GetDT(Institution institutionInstance, JobTitle jobTitleInstance) { return new DTEmployment(jobTitleInstance.Name, institutionInstance.Nickname, From, To, Created, State); }

        public EmploymentCardByInstitution GetCardByInstitution(Person personInstance, JobTitle jobTitleInstance) 
        { 
            return new EmploymentCardByInstitution(Id, jobTitleInstance.Name, personInstance.Nickname); 
        }

        public EmploymentCardByPerson GetCardByPerson(Institution institutionInstance, JobTitle jobTitleInstance) 
        {
            return new EmploymentCardByPerson(Id, jobTitleInstance.Name, institutionInstance.Nickname);
        }
    }
}
