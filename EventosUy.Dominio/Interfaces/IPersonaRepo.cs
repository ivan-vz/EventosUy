using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IPersonaRepo
    {
        public Task<Persona> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public Task AddAsync(Persona persona);
        public Task RemoveAsync(Guid id);
    }
}
