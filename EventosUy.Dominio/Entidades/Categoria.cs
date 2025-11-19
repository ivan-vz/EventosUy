namespace EventosUy.Dominio.Entidades
{
    public class Categoria
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        public Categoria(string nombre, string descripcion) 
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Descripcion = descripcion;
        }
    }
}
