namespace EventosUy.Dominio.Entidades
{
    public class Categoria
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public DateTimeOffset Creacion { get; private set; }
        public bool Activo { get; private set; }

        public Categoria(string nombre, string descripcion) 
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Descripcion = descripcion;
            Creacion = DateTimeOffset.UtcNow;
            Activo = true;
        }
    }
}
