using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IEventoRepo
    {
        public Task<Evento> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllPorInstitucionAsync(Guid id_institucion);
        public IEnumerable<Guid> GetCategoriasAsync(Guid id);
        public Task AddAsync(Evento evento);
        public Task RemoveAsync(Guid id);
    }
}
