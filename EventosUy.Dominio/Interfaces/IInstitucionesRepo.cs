using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IInstitucionesRepo
    {
        public Task<Institucion> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public Task AddAsync(Institucion institucion);
        public Task RemoveAsync(Guid id);
    }
}
