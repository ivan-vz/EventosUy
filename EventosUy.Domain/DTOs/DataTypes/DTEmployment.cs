using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTEmployment
    {
        public string JobTitle { get; init; }
        public string Institution { get; init; }
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public DateTimeOffset Created { get; init; }
        public EmploymentState State { get; init; }

        public DTEmployment(string jobTitle, string institution, DateOnly from, DateOnly to, DateTimeOffset created, EmploymentState state) 
        {
            JobTitle = jobTitle;
            Institution = institution;
            From = from;
            To = to;
            Created = created;
            State = state;
        }
    }
}
