namespace EventosUy.Dominio.Entidades
{
    public class Registro
    {
        public Guid Id { get; set; }
        public DateOnly Creacion { get; private set; }
        public float Costo { get; private set; }
        public string CodigoPatrocinio { get; private set; }
        public Guid Asistente { get; private set; }
        public Guid Edicion { get; private set; }
        public Guid TipoRegistro { get; private set; }

        public Registro(DateOnly creacion, float costo, string codigo, Guid id_asistente, Guid id_edicion, Guid id_tipoRegistro) 
        {
            Creacion = creacion;
            Costo = costo;
            CodigoPatrocinio = codigo;
            Asistente = id_asistente;
            Edicion = id_edicion;
            TipoRegistro = id_tipoRegistro;
        }
    }
}
