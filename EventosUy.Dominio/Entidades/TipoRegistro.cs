namespace EventosUy.Dominio.Entidades
{
    public class TipoRegistro
    {
        public Guid ID { get; private set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public float Costo { get; private set; }
        public int Cupos { get; private set; }
        public Guid Edicion { get; private set; }

        public TipoRegistro(string nombre, string descripcion, float costo, int cupos, Guid id_edicion) 
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Costo = costo;
            Cupos = cupos;
            Edicion = id_edicion;
        }
    }
}
