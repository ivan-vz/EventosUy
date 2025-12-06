using EventosUy.Domain.Common;
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

        private Register(float total, string sponsorCode, Guid personId, Guid editionId, Guid registerTypeId, Participation participation) 
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

        public static Result<Register> Create(float total, string sponsorCode, Guid personId, Guid editionId, Guid registerTypeId, Participation participation) 
        {
            if (total < 0) { return Result<Register>.Failure("Total must be greater than or equal to 0."); }
            if (participation == Participation.EMPLOYEE && total > 0) { return Result<Register>.Failure("Employees do not have to pay."); }
            if (participation == Participation.GUEST && total > 0) { return Result<Register>.Failure("Guests do not have to pay."); }

            Register registerInstance = new Register(total, sponsorCode, personId, editionId, registerTypeId, participation);

            return Result<Register>.Success(registerInstance);
        }

        public DTRegister GetDT(Edition editionInstance, RegisterType registerTypeInstance) 
        { 
            return new DTRegister(registerTypeInstance.Name, editionInstance.Name, Created, Total, SponsorCode, Participation, State); 
        }

        public RegisterCardByEdition GetCardByEdition(Person personInstance) { return new RegisterCardByEdition(Id, personInstance.Nickname, Participation); }

        public RegisterCardByPerson GetCardByPerson(Edition editionInstance) { return new RegisterCardByPerson(Id, editionInstance.Name, Participation); }
    }
}
