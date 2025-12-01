using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
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

        public void ResendRequest()
        {
            Request = DateTimeOffset.UtcNow;
            Response = DateTimeOffset.MinValue;
            State = RequestState.PENDING;
            // TODO: Agregar un sistema de notificacion?
        }

        public void UpdateRequest(Url linktree, HashSet<string> specialities)
        {
            Request = DateTimeOffset.UtcNow;
            Response = DateTimeOffset.MinValue;
            State = RequestState.PENDING;
            ResendRequest();
        }

        public bool IsVerify() { return RequestState.APPROVED.Equals(State); }

        public void Approve() { State = RequestState.APPROVED; }

        public void Reject() { State = RequestState.REJECTED; }

        public DTProfessionalProfile GetDT() { return new DTProfessionalProfile(LinkTree, Specialities, State, Person); }

        public ProfileCard GetCard(Person personInstance) { return new ProfileCard(Id, personInstance.Nickname, personInstance.Email.Value); }
    }
}
