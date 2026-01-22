using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.Interfaces
{
    public interface IRegisterService
    {
        public Task<Result<Guid>> CreateAsync(Guid personId, Guid editionId, Guid registerTypeId, Participation participation, string sponsorCode);
        public Task<Result<Guid>> CreateAsync(Guid personId, Guid editionId, Guid registerTypeId, Participation participation);
        public Task<Result<DTRegister>> GetDTAsync(Guid id);
        public Task<Result<List<RegisterCardByEdition>>> GetAllByEditionAsync(Guid editionId);
        public Task<Result<List<RegisterCardByClient>>> GetAllByPersonAsync(Guid personId);
    }
}
