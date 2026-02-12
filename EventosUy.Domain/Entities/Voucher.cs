using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Voucher(string name, string code, int discount, int quota, bool isAutoApplied, Guid editionId, Guid registerTypeId, Guid? sponsorId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public string Code { get; init; } = code;
        public int Discount { get; init; } = discount;
        public int Quota { get; init; } = quota;
        public int Used { get; set; } = 0;
        public bool IsAutoApplied { get; init; } = isAutoApplied;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.Now;
        public VoucherState State { get; set; } = VoucherState.AVAILABLE;
        public Guid EditionId { get; init; } = editionId;
        public Guid RegisterTypeId { get; init; } = registerTypeId;
        public Guid? SponsorId { get; init; } = sponsorId;
    }
}
