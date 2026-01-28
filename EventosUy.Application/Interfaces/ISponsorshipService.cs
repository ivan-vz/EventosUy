using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface ISponsorshipService
    {
        public Task<(DTSponsorship?, ValidationResult)> CreateAsync(DTInsertSponsorship dTInsert);
        public Task<DTSponsorship?> GetByIdAsync(Guid id);
        public Task<SponsorshipCard?> GetCardByIdAsync(Guid id);
        public Task<IEnumerable<SponsorshipCard>> GetAllByEditionAsync(Guid editionId);
        public Task<IEnumerable<SponsorshipCard>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<bool> ValidateCodeAsync(string code, Guid editionId, Guid registerTypeId);
        public Task<DTSponsorship?> DeleteAsync(Guid id);
    }
}
