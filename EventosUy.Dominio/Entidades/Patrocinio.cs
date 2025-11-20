using EventosUy.Dominio.Enumerados;

namespace EventosUy.Dominio.Entidades
{
    public class Patrocinio
    {
        public Guid Id { get; private set; }
        public DateOnly Creacion { get; private set; }
        public float Monto { get; private set; }
        public int Gratuitos { get; private set; }
        public int Consumidos { get; private set; }
        public string Codigo { get; private set; }
        public NivelPatrocinio Nivel { get; private set; }
        public EstadoPatrocinio Estado { get; private set; }
        public DateOnly Hasta { get; private set; }
        public Guid Institucion { get; private set; }
        public Guid Edicion { get; private set; }
        public Guid TipoRegistro { get; private set; }

        public Patrocinio(DateOnly creacion, float monto, int gratuitos, string codigo, NivelPatrocinio nivel, Guid id_institucion, Guid id_edicion, Guid id_tipoRegistro, DateOnly hasta)
        {
            Creacion = creacion;
            Monto = monto;
            Gratuitos = gratuitos;
            Consumidos = 0;
            Codigo = codigo;
            Nivel = nivel;
            Institucion = id_institucion;
            Edicion = id_edicion;
            TipoRegistro = id_tipoRegistro;
            Estado = EstadoPatrocinio.DISPONIBLE;
            Hasta = hasta;
        }
    }
}
