namespace EventosUy.Dominio.Entidades
{
    public class Evento
    {
        public Guid Id { get; init; }
        public string Nombre { get; init; }
        public string Sigla { get; init; }
        public string Descripcion { get; private set; }
        public DateTimeOffset Creacion { get; init; }
        public bool Activo { get; private set; }
        public HashSet<Guid> Categorias { get; private set; }
        public Guid? Institucion { get; init; }

        public Evento(string nombre, string sigla, string descripcion, Guid? institucion)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Sigla = sigla;
            Descripcion = descripcion;
            Creacion = DateTimeOffset.UtcNow;
            Activo = true;
            Categorias = [];
            Institucion = institucion;
        }

        public void AgregarCategoria(Guid id_categoria) { Categorias.Add(id_categoria); }
    }
}
