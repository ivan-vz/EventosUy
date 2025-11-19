using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventosUy.Dominio.Entidades
{
    public class Evento
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Sigla { get; private set; }
        public string Descripcion { get; private set; }
        public DateTimeOffset Creacion { get; private set; }
        public HashSet<Guid> Categorias { get; private set; }

        public Evento(string nombre, string sigla, string descripcion)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Sigla = sigla;
            Descripcion = descripcion;
            Creacion = DateTimeOffset.UtcNow;
            Categorias = new HashSet<Guid>();
        }

        public void AgregarCategoria(Guid id_categoria) { Categorias.Add(id_categoria); }
    }
}
