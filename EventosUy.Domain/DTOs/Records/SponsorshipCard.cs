using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.Records
{
    public record SponsorshipCard(Guid Id, string Name, DateOnly Until, SponsorshipTier Tier);
}
