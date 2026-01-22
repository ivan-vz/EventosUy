using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.Interfaces
{
    public interface ISponsorshipService
    {
        public Task<Result<Guid>> CreateAsync(string name, SponsorshipTier tier, decimal amount, DateOnly expiration, Guid editionId, Guid institutionId, Guid registerTypeId);
        public Task<Result<DTSponsorship>> GetDTAsync(Guid id);
        public Task<Result<Sponsorship>> GetByCodeAsync(string code);
        public Task<Result<List<SponsorshipCard>>> GetAllByEditionAsync(Guid editionId);
        public Task<Result<List<SponsorshipCard>>> GetAllByInstitutionAsync(Guid institutionId);
    }
}
