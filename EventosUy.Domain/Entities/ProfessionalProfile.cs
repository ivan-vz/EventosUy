using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class ProfessionalProfile
    {
        public Guid Id { get; init; }
        public Url LinkTree { get; private set; }
        public List<string> Specialities { get; private set; }
        public DateTimeOffset Request { get; private set; }
        public DateTimeOffset Response { get; private set; }
        public RequestState State { get; private set; }

        public ProfessionalProfile(Url linkTree, List<string> specialities) 
        {
            Id = Guid.NewGuid();
            LinkTree = linkTree;
            Specialities = specialities;
            Request = DateTimeOffset.Now;
            Response = DateTimeOffset.MinValue;
            State = RequestState.PENDING;
        }
    }
}
