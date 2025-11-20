using EventosUy.Dominio.Enumerados;

namespace EventosUy.Dominio.Entidades
{
    public class Empleo
    {
        public Guid Id { get; set; }
        public DateOnly Inicio { get; set; }
        public DateOnly Fin { get; set; }
        public EstadoEmpleo Estado { get; set; }
        public Guid Cargo { get; set; }
        public Guid Empleado { get; set; }
        public Guid Institucion { get; set; }

        public Empleo(DateOnly inicio, DateOnly fin, Guid id_cargo, Guid id_empleado, Guid id_institucion) 
        {
            Inicio = inicio;
            Fin = fin;
            Estado = EstadoEmpleo.ACTIVO;
            Cargo = id_cargo;
            Empleado = id_empleado;
            Institucion = id_institucion;
        }
    }
}
