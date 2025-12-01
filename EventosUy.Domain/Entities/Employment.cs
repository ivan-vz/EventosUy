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
        public Guid Person { get; init; }
        public Guid Institution { get; init; }

        public Employment(DateOnly from, DateOnly to, Guid jobTitleId, Guid personId, Guid institutionId) 
        {
            Created = DateTimeOffset.UtcNow;
            From = from;
            To = to;
            State = EmploymentState.ACTIVE;
            JobTitle = jobTitleId;
            Person = personId;
            Institution = institutionId;
        }
    }
}
