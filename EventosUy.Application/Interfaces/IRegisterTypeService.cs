using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;

namespace EventosUy.Application.Interfaces
{
    public interface IRegisterTypeService
    {
        public Task<Result<Guid>> CreateAsync(string name, string description, float price, int quota, Guid eventiId);
        public Task<Result<DTRegisterType>> GetDTAsync(Guid id);
        public Task<Result<RegisterType>> GetByIdAsync(Guid id);
        public Task<Result<List<RegisterTypeCard>>> GetAllByEditionAsync(Guid editionId);
    }
}
