using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IPatrocinioRepo
    {
        public Task<Patrocinio> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllPorEdicion(Guid id_edicion);
        public IEnumerable<Guid> GetAllPorInstitucion(Guid id_institucion);
        public IEnumerable<Guid> GetAllPorTipoRegistro(Guid id_tipoRegistro);
        public Task AddAsync(Patrocinio patrocinio);
        public Task RemoveAsync(Guid id);
    }
}
