using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface ICargoRepo
    {
        public Task<Cargo> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllPorInstitucionAsync(Guid id_institucion);
        public Task AddAsync(Cargo cargo);
        public Task RemoveAsync(Guid id);
    }
}
