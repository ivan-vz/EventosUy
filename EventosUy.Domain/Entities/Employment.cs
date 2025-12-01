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

        public Employment(DateOnly from, DateOnly to, Guid jobTitleId, Guid professionalId, Guid institutionId) 
        {
            Created = DateTimeOffset.UtcNow;
            From = from;
            To = to;
            State = EmploymentState.ACTIVE;
            JobTitle = jobTitleId;
            ProfessionalProfile = professionalId;
            Institution = institutionId;
        }

        public DTEmployment GetDT(Institution institutionInstance, JobTitle jobTitleInstance) { return new DTEmployment(jobTitleInstance.Name, institutionInstance.Nickname, From, To, Created, State); }

        public EmploymentCardByInstitution GetEmploymentCardByInstitution(Person personInstance, JobTitle jobTitleInstance) 
        { 
            return new EmploymentCardByInstitution(Id, jobTitleInstance.Name, personInstance.Nickname); 
        }

        public EmploymentCardByPerson GetEmploymentCardByPerson(Institution institutionInstance, JobTitle jobTitleInstance) 
        {
            return new EmploymentCardByPerson(Id, jobTitleInstance.Name, institutionInstance.Nickname);
        }
    }
}
