namespace EventosUy.Dominio.Entidades
{
    public class TipoRegistro
    {
        public Guid ID { get; init; }
        public string Nombre { get; init; }
        public string Descripcion { get; private set; }
        public float Costo { get; init; }
        public int Cupos { get; init; }
        public bool Activo { get; private set; }
        public DateTimeOffset Creacion { get; init; }
        public Guid Edicion { get; init; }

        public TipoRegistro(string nombre, string descripcion, float costo, int cupos, Guid id_edicion) 
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Costo = costo;
            Cupos = cupos;
            Edicion = id_edicion;
            Activo = true;
            Creacion = DateTimeOffset.UtcNow;
        }
    }
}
