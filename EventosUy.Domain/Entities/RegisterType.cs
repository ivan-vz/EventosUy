using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.Entities
{
    public class RegisterType
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; private set; }
        public float Price { get; init; }
        public int Quota { get; init; }
        public bool Active { get; private set; }
        public DateTimeOffset Created { get; init; }
        public Guid Edition { get; init; }

        private RegisterType(string name, string description, float price, int quota, Guid editionId) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Quota = quota;
            Edition = editionId;
            Active = true;
            Created = DateTimeOffset.UtcNow;
        }

        public static Result<RegisterType> Create(string name, string description, float price, int quota, Guid editionId) 
        {
            if (string.IsNullOrWhiteSpace(name)) { return Result<RegisterType>.Failure("Name can not be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { return Result<RegisterType>.Failure("Description can not be empty."); }
            if (price < 0) { return Result<RegisterType>.Failure("Price must be greater than or equal to 0."); }
            if (quota <= 0) { return Result<RegisterType>.Failure("Quota must be greater than 0."); }

            RegisterType registerTypeInstance = new RegisterType(name, description, price, quota, editionId);

            return Result<RegisterType>.Success(registerTypeInstance);
        }

        public DTRegisterType GetDT(Edition editionInstance) { return new DTRegisterType(Name, editionInstance.Name, Description, Price, Quota, Created); }

        public RegisterTypeCard GetCard() { return new RegisterTypeCard(Id, Name, Active); }
    }
}
