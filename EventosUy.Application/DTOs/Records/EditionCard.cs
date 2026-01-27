using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.Records
{
    public record EditionCard(Guid Id, string Name, string Initials, EditionState State);
}
