using EventosUy.Dominio.Enumerados;

namespace EventosUy.Domain.DTOs.Records
{
    public  record RegisterCardByPerson(Guid Id, string Edition, Participation Participation);
}
