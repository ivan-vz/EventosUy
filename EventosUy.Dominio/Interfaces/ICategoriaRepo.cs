using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface ICategoriaRepo
    {
        public Task<Categoria> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public Task AddAsync(Categoria categoria);
        public Task RemoveAsync(Guid id);
    }
}
