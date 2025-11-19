using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Institucion : Usuario
    {
        public string Descripcion { get; private set; }
        public Direccion Direccion { get; private set; }
        public Url Url { get; private set; }

        public HashSet<Guid> Asistentes { get; private set; }

        public Institucion(string descripcion, Direccion direccion, Url url, string nickname, string password, string nombre, Email email) 
            : base(nickname, password, nombre, email)
        {
            Descripcion = descripcion;
            Direccion = direccion;
            Url = url;
            Asistentes = new HashSet<Guid>();
        }

        public void AgregarAsistente(Guid id_asistente) { Asistentes.Add(id_asistente); }
    }
}
