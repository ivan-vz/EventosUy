using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Persona : Usuario
    {
        public string Apellido { get; private set; }
        public DateOnly Nacimiento { get; private set; }

        public Persona(string apellido, DateOnly nacimiento, string nickname, string password, string nombre, Email email) 
            : base(nickname, password, nombre, email)
        {
            Apellido = apellido;
            Nacimiento = nacimiento;
        }
    }
}
