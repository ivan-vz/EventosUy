namespace EventosUy.Dominio.Entidades
{
    public class Categoria
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; }
        public string Descripcion { get; private set; }
        public DateTimeOffset Creacion { get; init; }
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
