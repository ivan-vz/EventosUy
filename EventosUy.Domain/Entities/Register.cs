namespace EventosUy.Domain.Entities
{
    public class Register(decimal total, Guid clientId, Guid editionId, Guid registerTypeId, Guid? voucherId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public decimal Total { get; init; } = total;
        public Guid Client { get; init; } = clientId;
        public Guid Edition { get; init; } = editionId;
        public Guid RegisterType { get; init; } = registerTypeId;
        public Guid? Voucher { get; init; } = voucherId;
    }
}
