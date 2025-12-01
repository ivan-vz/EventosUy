using EventosUy.Dominio.Enumerados;

namespace EventosUy.Domain.DTOs.Records
{
    public record SponsorshipCard(Guid Id, string Name, DateOnly Until, SponsorshipTier Tier);
}
