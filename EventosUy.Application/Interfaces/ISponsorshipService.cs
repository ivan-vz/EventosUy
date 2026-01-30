using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using FluentValidation.Results;

namespace EventosUy.Application.Interfaces
{
    public interface ISponsorshipService
    {
        public Task<(DTSponsorship? dt, ValidationResult validation)> CreateAsync(DTInsertSponsorship dTInsert);
        public Task<(DTSponsorship? dt, SponsorshipCard? card)> GetByIdAsync(Guid id);
        public Task<IEnumerable<SponsorshipCard>> GetAllByEditionAsync(Guid editionId);
        public Task<IEnumerable<SponsorshipCard>> GetAllByInstitutionAsync(Guid institutionId);
        public Task<DTSponsorship?> DeleteAsync(Guid id);
    }
}
