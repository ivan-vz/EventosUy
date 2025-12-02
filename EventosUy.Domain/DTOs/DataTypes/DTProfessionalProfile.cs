using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTProfessionalProfile
    {
        public Url LinkTree { get; init; }
        public HashSet<String> Specialities { get; init; }
        public DateTimeOffset Request { get; private set; }
        public RequestState State { get; private set; }

        public DTProfessionalProfile(Url linkTree, DateTimeOffset request, HashSet<string> specialities, RequestState state)
        {
            LinkTree = linkTree;
            Request = request;
            Specialities = specialities;
            State = state;
        }
    }
}
