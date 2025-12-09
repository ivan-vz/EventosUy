using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.Interfaces
{
    public interface ISponsorshipService
    {
        public Task<Result<Guid>> CreateAsync(string name, SponsorshipTier tier, float amount, int free, string code, DateOnly expiration, Guid editionId, Guid institutionId, Guid registerTypeId);
        public Task<Result<DTSponsorship>> GetDTAsync(Guid id);
        public Task<Result<List<SponsorshipCard>>> GetAllByEditionAsync(Guid editionId);
        public Task<Result<List<SponsorshipCard>>> GetAllByInstitutionAsync(Guid institutionId);
        Task<Result> ValidateCodeAsync(Guid registerTypeId, string sponsorCode);
    }
}
