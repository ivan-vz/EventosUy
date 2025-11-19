using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Institucion : Usuario
    {
        public string Descripcion { get; private set; }
        public Direccion Direccion { get; private set; }
        public Url Url { get; private set; }

        public HashSet<Guid> Asistentes { get; private set; }
        public Dictionary<Guid, Guid> Patrocinios { get; private set; } // clave = id_edicion, valor = id_patrocinio

        public Institucion(string descripcion, Direccion direccion, Url url, string nickname, string password, string nombre, Email email) 
            : base(nickname, password, nombre, email)
        {
            Descripcion = descripcion;
            Direccion = direccion;
            Url = url;
            Asistentes = [];
            Patrocinios = [];
        }

        public void AgregarAsistente(Guid id_asistente) { Asistentes.Add(id_asistente); }
        public void AgregarPatrocinio(Guid id_edicion, Guid id_patrocinio) { Patrocinios.TryAdd(id_edicion, id_patrocinio); }
    }
}
