using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IJobTitleService
    {
        public Task<Result<Guid>> CreateAsync(string name, string descripcion, Guid institutionId);
        public Task<Result<JobTitle>> GetByIdAsync(Guid id);
        public Task<Result<List<JobTitleCard>>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<Result<DTJobTitle>> GetDTAsync(Guid id);
    }
}
