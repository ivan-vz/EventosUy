using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.Records
{
    public record SponsorshipCard(Guid Id, string Name, SponsorshipTier Tier);
}
