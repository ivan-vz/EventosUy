using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using System.Reflection.Emit;

namespace EventosUy.Domain.Entities
{
    public class RegisterType
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; private set; }
        public decimal Price { get; init; }
        public int Quota { get; init; }
        public int Used { get; private set; }
        public bool Active { get; private set; }
        public DateTimeOffset Created { get; init; }
        public Guid Edition { get; init; }

        private RegisterType(string name, string description, decimal price, int quota, Guid editionId) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Quota = quota;
            Used = 0;
            Edition = editionId;
            Active = true;
            Created = DateTimeOffset.UtcNow;
        }

        public static Result<RegisterType> Create(string name, string description, decimal price, int quota, Guid editionId) 
        {
            List<string> errors = [] ;
            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name cannot be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { errors.Add("Description cannot be empty."); }
            if (price < 0) { errors.Add("Price must be greater than or equal to 0."); }
            if (quota <= 0) { errors.Add("Quota must be greater than 0."); }

            if (errors.Count != 0) { return Result<RegisterType>.Failure(errors); }

            RegisterType registerTypeInstance = new(name, description, price, quota, editionId);

            return Result<RegisterType>.Success(registerTypeInstance);
        }

        public DTRegisterType GetDT(Edition editionInstance) { return new(Name, editionInstance.GetCard(), Description, Price, Quota, Created); }

        public RegisterTypeCard GetCard() { return new(Id, Name, Active); }

        public bool IsActive() { return Active; }

        public Result UseSpot() 
        {
            if (Used >= Quota) { return Result.Failure("No available spots."); }

            Used++;

            if (Used == Quota) { Active = false; }

            return Result.Success();
        }
    }
}
