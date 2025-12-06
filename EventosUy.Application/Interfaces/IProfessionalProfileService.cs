using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    internal interface IProfessionalProfileService
    {
        Task<Result<ProfessionalProfile>> GetByIdAsync(Guid professionalId);
    }
}
