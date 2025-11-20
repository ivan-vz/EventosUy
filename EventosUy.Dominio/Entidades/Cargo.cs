namespace EventosUy.Dominio.Entidades
{
    public class Cargo
    {
        public Guid Id { get; set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public DateTimeOffset Creacion { get; private set; }
        public bool Activo { get; private set; }
        public Guid Institucion { get; private set; }

        public Cargo(string nombre, string descripcion, Guid id_institucion) 
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Creacion = DateTimeOffset.UtcNow;
            Activo = true;
            Institucion = id_institucion;
        }
    }
}
