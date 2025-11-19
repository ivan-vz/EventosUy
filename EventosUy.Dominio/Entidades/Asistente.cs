using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Asistente : Usuario
    {
        public string Apellido { get; private set; }
        public DateOnly Nacimiento { get; private set; }
        public Guid? Institucion { get; private set; }

        public Asistente(string apellido, DateOnly nacimiento, string nickname, string password, string nombre, Email email) 
            : base(nickname, password, nombre, email)
        {
            Apellido = apellido;
            Nacimiento = nacimiento;
            Institucion = null;
        }

        public void VincularInstitucion(Guid id_institucion) { Institucion = id_institucion; }
    }
}
