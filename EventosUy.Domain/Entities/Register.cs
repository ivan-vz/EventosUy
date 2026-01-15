using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Domain.Entidades
{
    public class Register
    {
        public Guid Id { get; init; }
        public DateTimeOffset Created { get; init; }
        public decimal Total { get; init; }
        public string? SponsorCode { get; init; }
        public Guid Client { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        private Register(decimal total, string? sponsorCode, Guid clientId, Guid editionId, Guid registerTypeId) 
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.UtcNow;
            Total = total;
            SponsorCode = sponsorCode;
            Person = personId;
            Edition = editionId;
            RegisterType = registerTypeId;
            Participation = participation;
            State = RegisterState.CONFIRMED;
        }

        public static Result<Register> Create(Guid personId, Guid editionId, RegisterType registerTypeInstance, Participation participation) 
        {
            List<string> errors = [];
            if (participation != Participation.CLIENT) { errors.Add($"Invalid participation: {participation}."); }
            if (!registerTypeInstance.IsActive()) { errors.Add($"Register type {registerTypeInstance.Name} is already full."); }

            if (errors.Any()) { return Result<Register>.Failure(errors); }

            decimal total = (participation == Participation.CLIENT) ? registerTypeInstance.Price : 0;

            Register registerInstance = new Register(total, null, personId, editionId, registerTypeInstance.Id, participation);

            return Result<Register>.Success(registerInstance);
        }

        public static Result<Register> Create(string sponsorCode, Guid personId, Guid editionId, RegisterType registerTypeInstance, Participation participation)
        {
            List<string> errors = [];
            if (participation == Participation.CLIENT) { errors.Add($"Invalid participation: {participation}."); }
            if (!registerTypeInstance.IsActive()) { errors.Add($"Register type {registerTypeInstance.Name} is already full."); }

            if (errors.Any()) { return Result<Register>.Failure(errors); }

            Register registerInstance = new Register(0, sponsorCode, personId, editionId, registerTypeInstance.Id, participation);

            return Result<Register>.Success(registerInstance);
        }

        public DTRegister GetDT(Edition editionInstance, RegisterType registerTypeInstance) 
        { 
            return new DTRegister(registerTypeInstance.Name, editionInstance.Name, Created, Total, SponsorCode, Participation, State); 
        }

        public RegisterCardByEdition GetCardByEdition(Client personInstance) { return new RegisterCardByEdition(Id, personInstance.Nickname, Participation); }

        public RegisterCardByClient GetCardByPerson(Edition editionInstance) { return new RegisterCardByClient(Id, editionInstance.Name, Participation); }
    }
}
