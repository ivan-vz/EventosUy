using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IEventService //Esto es publico para que se pueda ver de todos lados
    {
        public Task<Result<Event>> GetByIdAsync(Guid id);
    }
}
