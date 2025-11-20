using EventosUy.Dominio.Enumerados;

namespace EventosUy.Dominio.Entidades
{
    public class Patrocinio
    {
        public Guid Id { get; init; }
        public DateOnly Creacion { get; init; }
        public float Monto { get; init; }
        public int Gratuitos { get; init; }
        public int Consumidos { get; private set; }
        public string Codigo { get; init; }
        public NivelPatrocinio Nivel { get; init; }
        public EstadoPatrocinio Estado { get; private set; }
        public DateOnly Hasta { get; private set; }
        public Guid Institucion { get; init; }
        public Guid Edicion { get; init; }
        public Guid TipoRegistro { get; init; }

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
