using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Voucher(string name, string code, int discount, int quota, bool automatic, Guid editionId, Guid registerTypeId, Guid? sponsorId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public string Code { get; init; } = code;
        public int Discount { get; init; } = discount;
        public int Quota { get; init; } = quota;
        public int Used { get; set; } = 0;
        public bool IsAutoApplied { get; init; } = automatic;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.Now;
        public VoucherState State { get; set; } = VoucherState.AVAILABLE;
        public Guid Edition { get; init; } = editionId;
        public Guid RegisterType { get; init; } = registerTypeId;
        public Guid? Sponsor { get; init; } = sponsorId;
    }
}
