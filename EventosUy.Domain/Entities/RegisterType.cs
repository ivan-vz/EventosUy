using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.Entities
{
    public class RegisterType(string name, string description, decimal price, int quota, Guid editionId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public string Description { get; init; } = description;
        public decimal Price { get; init; } = price;
        public int Quota { get; init; } = quota;
        public int Used { get; private set; } = 0;
        public bool Active { get; set; } = true;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public Guid Edition { get; init; } = editionId;

        // TODO: Esto iria en el servicio
        public Result UseSpot() 
        {
            if (Used >= Quota) { return Result.Failure("No available spots."); }

            Used++;

            if (Used == Quota) { Active = false; }

            return Result.Success();
        }
    }
}
