using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.Records
{
    public record RegisterCardByEdition(Guid Id, string Person, Participation Participation);
}
