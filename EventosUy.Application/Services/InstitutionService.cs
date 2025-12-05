using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;

namespace EventosUy.Application.Services
{
    internal class InstitutionService : IInstitutionService
    {
        private readonly IInstitutionRepo _institutionRepo;

        public InstitutionService(IInstitutionRepo institutionRepo) { _institutionRepo = institutionRepo; }

        public async Task<Result<Institution>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<Institution>.Failure("Institution can not be empty."); }
            Institution? institutionInstance = await _institutionRepo.GetByIdAsync(id);

            if (institutionInstance is null) { return Result<Institution>.Failure("Institution not Found."); }

            return Result<Institution>.Success(institutionInstance);
        }
    }
}
