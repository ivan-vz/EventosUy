using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    internal interface IInstitutionService
    {
        public Task<Result<Institution>> GetByIdAsync(Guid id);
    }
}
