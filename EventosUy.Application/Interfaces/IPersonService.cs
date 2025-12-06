using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    internal interface IPersonService
    {
        Task<Result<Person>> GetByIdAsync(Guid person);
    }
}
