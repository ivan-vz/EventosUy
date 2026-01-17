using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Register
    {
        public Guid Id { get; init; }
        public DateTimeOffset Created { get; init; }
        public decimal Total { get; init; }
        public string? DiscountCode { get; init; }
        public Guid Client { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        private Register(decimal total, string? discountCode, Guid clientId, Guid editionId, Guid registerTypeId) 
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.UtcNow;
            Total = total;
            DiscountCode = discountCode;
            Client = clientId;
            Edition = editionId;
            RegisterType = registerTypeId;
        }

        public static Result<Register> Create(Client client, Edition edition, RegisterType registerType)
        {
            List<string> errors = [];

            if (!EditionState.ONGOING.Equals(edition.State)) { errors.Add($"Editions {edition.Name} is not ongoing."); }

            if (!registerType.Active) { errors.Add($"Register type {registerType.Name} is already full."); }
            if (registerType.Edition != edition.Id) { errors.Add($"Register type {registerType.Name} doesn't belong to Edition {edition.Name}."); }

            if (!UserState.ACTIVE.Equals(client.State)) { errors.Add($"User {client.Nickname} is not active."); }

            if (errors.Count != 0) { return Result<Register>.Failure(errors); }

            Register registerInstance = new(registerType.Price, null, client.Id, edition.Id, registerType.Id);

            return Result<Register>.Success(registerInstance);
        }

        public static Result<Register> Create(Voucher voucher, Client client, Edition edition, RegisterType registerType)
        {
            List<string> errors = [];

            if (!EditionState.ONGOING.Equals(edition.State)) { errors.Add($"Editions {edition.Name} is not ongoing."); }

            if (!registerType.Active) { errors.Add($"Register type {registerType.Name} is already full."); }
            if (registerType.Edition != edition.Id) { errors.Add($"Register type {registerType.Name} doesn't belong to Edition {edition.Name}."); }

            if (!VoucherState.AVAILABLE.Equals(voucher.State)) { errors.Add($"Voucher {voucher.Name} is not available."); }
            if (voucher.Edition != edition.Id) { errors.Add($"Voucher {voucher.Name} is not valid for Edition {edition.Name}."); }
            if (voucher.RegisterType != registerType.Id) { errors.Add($"Voucher {voucher.Name} is not valid for Register Type {registerType.Name}."); }

            if (!UserState.ACTIVE.Equals(client.State)) { errors.Add($"User {client.Nickname} is not active."); }

            if (errors.Count != 0) { return Result<Register>.Failure(errors); }

            decimal discount = registerType.Price * voucher.Discount / 100;
            decimal price = registerType.Price - discount;

            Register registerInstance = new(price, voucher.Code, client.Id, edition.Id, registerType.Id);

            return Result<Register>.Success(registerInstance);
        }

        public DTRegister GetDT(Edition edition, RegisterType registerType, Client client, Voucher? voucher) 
        { 
            return new(Created, Total, registerType!.GetCard(), edition!.GetCard(), client!.GetCard(), voucher?.GetCard()); 
        }

        public RegisterCardByEdition GetCardByEdition(Client client) { return new (Id, client.Nickname, Created); }

        public RegisterCardByClient GetCardByClient(Edition edition) { return new(Id, edition.Name, Created); }
    }
}
