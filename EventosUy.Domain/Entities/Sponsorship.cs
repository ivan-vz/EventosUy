using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship(string name, decimal amount, SponsorshipTier tier, Guid editionId, Guid institutionId, Guid registerTypeId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public decimal Amount { get; init; } = amount;
        public SponsorshipTier Tier { get; init; } = tier;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public bool Active { get; set; } = true;
        public Guid EditionId { get; init; } = editionId;
        public Guid InstitutionId { get; init; } = institutionId;
        public Guid RegisterTypeId { get; init; } = registerTypeId;
    }
}
