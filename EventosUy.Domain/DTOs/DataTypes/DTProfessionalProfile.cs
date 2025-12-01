using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTProfessionalProfile
    {
        public Guid Id { get; init; }
        public Url LinkTree { get; init; }
        public HashSet<String> Specialities { get; init; }
        public DateTimeOffset Request { get; private set; }
        public DateTimeOffset Response { get; private set; }
        public RequestState State { get; private set; }
        public Guid Person {  get; init; }

        public DTProfessionalProfile(Url linkTree, HashSet<string> specialities, RequestState state, Guid personId)
        {
            Id = Guid.NewGuid();
            LinkTree = linkTree;
            Specialities = specialities;
            State = state;
            Person = personId;
        }
    }
}
