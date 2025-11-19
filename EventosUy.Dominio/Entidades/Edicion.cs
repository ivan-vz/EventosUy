using EventosUy.Dominio.ValueObjects;

namespace EventosUy.Dominio.Entidades
{
    public class Edicion
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Siglas { get; private set; }
        public DateOnly Inicio { get; private set; }
        public DateOnly Fin { get; private set; }
        public DateOnly Creacion { get; private set; }
        public Direccion Direccion { get; private set; }
        public Guid Evento { get; private set; }
        public Guid Institucion { get; private set; }
        public HashSet<Guid> TiposRegistro { get; private set; }
        public Dictionary<Guid, Guid> Patrocinios { get; private set; } // clave = id_institucion, valor = id_patrocinio

        public Edicion(string nombre, string siglas, DateOnly inicio, DateOnly fin, DateOnly creacion, Direccion direccion, Guid id_evento, Guid id_institucion) 
        {
            Nombre = nombre;
            Siglas = siglas;
            Inicio = inicio;
            Fin = fin;
            Creacion = creacion;
            Direccion = direccion;
            Evento = id_evento;
            Institucion = id_institucion;
            TiposRegistro = [];
            Patrocinios = [];
        }

        public void AgregarTipoRegistro(Guid id_tipoRegistro) { TiposRegistro.Add(id_tipoRegistro); }
        public void AgregarPatrocinio(Guid id_institucion, Guid id_patrocinio) { Patrocinios.TryAdd(id_institucion, id_patrocinio); }
    }

}
