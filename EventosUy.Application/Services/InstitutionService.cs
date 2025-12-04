using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Services
{
    internal class InstitutionService : IInstitutionService
    {
        public Task<Result<Event>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
