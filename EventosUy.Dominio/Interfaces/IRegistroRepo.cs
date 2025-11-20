using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IRegistroRepo
    {
        public Task<Registro> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> getAllAsync();
        public IEnumerable<Guid> getAllPorPersonaAsync(Guid id_persona);
        public IEnumerable<Guid> getAllPorEdicionAsync(Guid id_edicion);
        public IEnumerable<Guid> getAllPorTipoRegistroAsync(Guid id_tipoRegistro);
        public Task AddAsync(Registro registro);
        public Task RemoveAsync(Guid id);
    }
}
