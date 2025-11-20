using EventosUy.Dominio.Enumerados;
using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public abstract class Usuario
    {
        public Guid Id { get; init; }
        public string Nickname { get; private set; }
        public string Password { get; private set; }
        public string Nombre { get; init; }
        public Email Email { get; private set; }
        public DateOnly Desde { get; init; }
        public EstadoUsuario Estado { get; set; }

        protected Usuario(string nickname, string password, string nombre, Email email) 
        {
            Id = Guid.NewGuid();
            Nickname = nickname;
            Password = password;
            Nombre = nombre;
            Email = email;
            Desde = DateOnly.FromDateTime(DateTime.UtcNow);
            Estado = EstadoUsuario.ACTIVO;
        }
    }
}
