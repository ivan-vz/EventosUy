using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.Interfaces
{
    public interface IRegisterService
    {
        public Task<Result<Guid>> CreateAsync(Guid personId, Guid editionId, Guid registerTypeId, string sponsorCode, float total, Participation participation);
        public Task<Result<DTRegister>> GetDTAsync(Guid id);
        public Task<Result<List<RegisterCardByEdition>>> GetAllByEditionAsync(Guid editionId);
        public Task<Result<List<RegisterCardByPerson>>> GetAllByPersonAsync(Guid personId);
    }
}
