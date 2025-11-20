using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IEdicionRepo
    {
        public Task<Edicion> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllPorInstitucionAsync(Guid id_institucion);
        public IEnumerable<Guid> GetAllPorEventoAsync(Guid id_evento);
        public Task AddAsync(Edicion edicion);
        public Task RemoveAsync(Guid id);
    }
}
