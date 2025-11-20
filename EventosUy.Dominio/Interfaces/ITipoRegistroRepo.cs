using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface ITipoRegistroRepo
    {
        public Task<TipoRegistro> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllPorEdicion(Guid id_edicion);
        public Task AddAsync(TipoRegistro tipoRegistro);
        public Task RemoveAsync(Guid id);
    }
}
