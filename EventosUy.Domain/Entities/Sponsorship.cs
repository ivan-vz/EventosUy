using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship(string name, decimal amount, SponsorshipTier tier, Guid edition, Guid institution, Guid registerType)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public decimal Amount { get; init; } = amount;
        public SponsorshipTier Tier { get; init; } = tier;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public bool Active { get; set; } = true;
        public Guid Edition { get; init; } = edition;
        public Guid Institution { get; init; } = institution;
        public Guid RegisterType { get; init; } = registerType;
    }
}
