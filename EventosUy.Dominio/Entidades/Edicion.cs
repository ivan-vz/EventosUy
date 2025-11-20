using EventosUy.Dominio.Enumerados;
using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Edicion
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; }
        public string Siglas { get; init; }
        public DateOnly Inicio { get; private set; }
        public DateOnly Fin { get; private set; }
        public DateOnly Creacion { get; init; }
        public Direccion Direccion { get; private set; }
        public EstadoEdicion Estado { get; private set; }
        public Guid Evento { get; init; }
        public Guid Institucion { get; init; }

        public Edicion(string nombre, string siglas, DateOnly inicio, DateOnly fin, DateOnly creacion, Direccion direccion, Guid id_evento, Guid id_institucion) 
        {
            Nombre = nombre;
            Siglas = siglas;
            Inicio = inicio;
            Fin = fin;
            Creacion = creacion;
            Direccion = direccion;
            Estado = EstadoEdicion.BORRADOR;
            Evento = id_evento;
            Institucion = id_institucion;
        }
    }

}
