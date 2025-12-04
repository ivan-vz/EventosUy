using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Services
{
    public class EventService : IEventService //Esto queda internal asi queda privado y no se ve desde otros proyectos "EventosUy."
    {
        public Task<Result<Event>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
