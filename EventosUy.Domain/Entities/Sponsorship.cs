using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTimeOffset Created { get; init; }
        public float Amount { get; init; }
        public int Free { get; init; }
        public int Used { get; private set; }
        public string Code { get; init; }
        public SponsorshipTier Tier { get; init; }
        public SponsorshipState State { get; private set; }
        public DateOnly Expired { get; private set; }
        public Guid Institution { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        public Sponsorship(string name, float amount, int free, string code, SponsorshipTier tier, Guid institutionId, Guid editionId, Guid registerTypeId, DateOnly expired)
        {
            Name = name;
            Created = DateTimeOffset.UtcNow;
            Amount = amount;
            Free = free;
            Used = 0;
            Code = code;
            Tier = tier;
            Institution = institutionId;
            Edition = editionId;
            RegisterType = registerTypeId;
            State = SponsorshipState.AVAILABLE;
            Expired = expired;
        }
    }
}
