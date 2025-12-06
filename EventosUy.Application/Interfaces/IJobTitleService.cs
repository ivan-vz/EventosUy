using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    internal interface IJobTitleService
    {
        Task<Result<JobTitle>> GetByIdAsync(Guid jobTitle);
    }
}
