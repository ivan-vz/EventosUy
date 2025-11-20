using EventosUy.Dominio.Enumerados;

namespace EventosUy.Dominio.Entidades
{
    public class Registro
    {
        public Guid Id { get; set; }
        public DateOnly Creacion { get; private set; }
        public float Costo { get; private set; }
        public string CodigoPatrocinio { get; private set; }
        public Participacion Participacion { get; private set; }
        public EstadoAsistencia Estado { get; private set; }
        public Guid Persona { get; private set; }
        public Guid Edicion { get; private set; }
        public Guid TipoRegistro { get; private set; }

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
