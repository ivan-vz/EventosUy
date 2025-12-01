using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Institucion : Usuario
    {
        public string Descripcion { get; private set; }
        public Address Direccion { get; private set; }
        public Url Url { get; private set; }

        public Institucion(string descripcion, Address direccion, Url url, string nickname, string password, string nombre, Email email) 
            : base(nickname, password, nombre, email)
        {
            Descripcion = descripcion;
            Direccion = direccion;
            Url = url;
        }
    }
}
