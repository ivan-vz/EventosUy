using EventosUy.Dominio.Enumerados;

namespace EventosUy.Dominio.Entidades
{
    public class Registro
    {
        public Guid Id { get; init; }
        public DateOnly Creacion { get; init; }
        public float Costo { get; init; }
        public string CodigoPatrocinio { get; init; }
        public Participacion Participacion { get; init; }
        public EstadoAsistencia Estado { get; private set; }
        public Guid Persona { get; init; }
        public Guid Edicion { get; init; }
        public Guid TipoRegistro { get; init; }

        public Registro(DateOnly creacion, float costo, string codigo, Guid id_persona, Guid id_edicion, Guid id_tipoRegistro, Participacion participacion) 
        {
            Creacion = creacion;
            Costo = costo;
            CodigoPatrocinio = codigo;
            Persona = id_persona;
            Edicion = id_edicion;
            TipoRegistro = id_tipoRegistro;
            Participacion = participacion;
            Estado = EstadoAsistencia.CONFIRMADO;
        }
    }
}
