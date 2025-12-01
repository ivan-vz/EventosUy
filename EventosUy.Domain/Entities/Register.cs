using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entidades
{
    public class Register
    {
        public Guid Id { get; init; }
        public DateTimeOffset Created { get; init; }
        public float Total { get; init; }
        public string SponsorCode { get; init; }
        public Participation Participation { get; init; }
        public RegisterState State { get; private set; }
        public Guid Person { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        public Register(float total, string sponsorCode, Guid personId, Guid editionId, Guid registerTypeId, Participation participation) 
        {
            Created = DateTimeOffset.UtcNow;
            Total = total;
            SponsorCode = sponsorCode;
            Person = personId;
            Edition = editionId;
            RegisterType = registerTypeId;
            Participation = participation;
            State = RegisterState.CONFIRMED;
        }

        public DTRegister GetDT(Edition editionInstance, RegisterType registerTypeInstance) 
        { 
            return new DTRegister(registerTypeInstance.Name, editionInstance.Name, Created, Total, SponsorCode, Participation, State); 
        }

        public RegisterCardByEdition GetCardByEdition(Person personInstance) { return new RegisterCardByEdition(Id, personInstance.Nickname, Participation); }

        public RegisterCardByPerson GetRegisterCardByPerson(Edition editionInstance) { return new RegisterCardByPerson(Id, editionInstance.Name, Participation); }
    }
}
