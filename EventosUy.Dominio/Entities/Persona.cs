using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Persona : Usuario
    {
        public string Apellido { get; init; }
        public DateOnly Nacimiento { get; init; }

        public Persona(string apellido, DateOnly nacimiento, string nickname, string password, string nombre, Email email) 
            : base(nickname, password, nombre, email)
        {
            Apellido = apellido;
            Nacimiento = nacimiento;
        }
    }
}
