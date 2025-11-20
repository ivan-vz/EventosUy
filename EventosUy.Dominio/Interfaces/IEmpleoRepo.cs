using EventosUy.Dominio.Entidades;

namespace EventosUy.Dominio.Interfaces
{
    public interface IEmpleoRepo
    {
        public Task<Empleo> GetPorIdAsync(Guid id);
        public IEnumerable<Guid> GetAllAsync();
        public IEnumerable<Guid> GetAllPorEmpleado(Guid id_empleado);
        public IEnumerable<Guid> GetAllPorInstitucion(Guid id_institucion);
        public IEnumerable<Guid> GetAllPorCargo(Guid id_cargo);
        public Task AddAsync(Empleo empleo);
        public Task RemoveAsync(Guid id);
    }
}
