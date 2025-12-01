namespace EventosUy.Dominio.Entidades
{
    public class Cargo
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; }
        public string Descripcion { get; private set; }
        public DateTimeOffset Creacion { get; init; }
        public bool Activo { get; private set; }
        public Guid Institucion { get; init; }

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
