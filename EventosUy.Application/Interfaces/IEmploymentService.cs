using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.Interfaces
{
    public interface IEmploymentService
    {
        public Task<Result<Guid>> CreateAsync(DateOnly from, DateOnly to, Guid institutionId, Guid professionalId, Guid jobTitleId);
        public Task<Result<DTEmployment>> GetDTAsync(Guid id);
        public Task<Result<List<EmploymentCardByInstitution>>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<Result<List<EmploymentCardByPerson>>> GetAllByPersonAsync(Guid personId);
        public Task<Result> HasActiveContractAsync(Guid person, Guid institution);
    }
}
