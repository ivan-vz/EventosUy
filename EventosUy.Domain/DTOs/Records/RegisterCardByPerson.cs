using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.Records
{
    public  record RegisterCardByPerson(Guid Id, string Edition, Participation Participation);
}
