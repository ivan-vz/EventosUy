using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Application.Services
{
    internal class SponsorshipService : ISponsorshipService
    {
        private readonly ISponsorshipRepo _repo;
        private readonly IEditionService _editionService;
        private readonly IInstitutionService _institutionService;
        private readonly IRegisterTypeService _registerTypeService;

        public SponsorshipService(ISponsorshipRepo sponsorship, IEditionService editionService, IInstitutionService institutionService, IRegisterTypeService registerTypeService) 
        {
            _repo = sponsorship;
            _editionService = editionService;
            _institutionService = institutionService;
            _registerTypeService = registerTypeService;
        }

        public async Task<Result<Guid>> CreateAsync(string name, SponsorshipTier tier, decimal amount, DateOnly expiration, Guid editionId, Guid institutionId, Guid registerTypeId)
        {
            List<string> errors = [];
            Result<Edition> editionResult = await _editionService.GetByIdAsync(editionId);
            if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { errors.AddRange(institutionResult.Errors); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerTypeId);
            if (!registerTypeResult.IsSuccess) { errors.AddRange(registerTypeResult.Errors); }

            Result<SponsorLevel> sponsorLevelResult = SponsorLevel.Create(amount, tier, registerTypeResult.Value!.Price);
            if (!sponsorLevelResult.IsSuccess) { errors.AddRange(sponsorLevelResult.Errors); }

            if (errors.Any()) { return Result<Guid>.Failure(errors); }

            if (await _repo.ExistsAsync(editionId, institutionId)) { return Result<Guid>.Failure("Sponsorship already exist."); }

            Result<Sponsorship> sponsorResult = Sponsorship.Create(name, sponsorLevelResult.Value!, institutionResult.Value!, editionResult.Value!, registerTypeResult.Value!, expiration);
            if (!sponsorResult.IsSuccess) { return Result<Guid>.Failure(sponsorResult.Errors); }

            Sponsorship sponsorInstance = sponsorResult.Value!;
            await _repo.AddAsync(sponsorInstance);

            return Result<Guid>.Success(sponsorInstance.Id);
        }

        public async Task<Result<List<SponsorshipCard>>> GetAllByEditionAsync(Guid editionId)
        {
            if (editionId == Guid.Empty) { return Result<List<SponsorshipCard>>.Failure("Edition can not be empty."); }
            List<Sponsorship> sponsorships = await _repo.GetAllByEditionAsync(editionId);
            List<SponsorshipCard> cards = sponsorships.Select(sponsor => sponsor.GetCard()).ToList();

            return Result<List<SponsorshipCard>>.Success(cards);
        }

        public async Task<Result<List<SponsorshipCard>>> GetAllByInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty) { return Result<List<SponsorshipCard>>.Failure("Institution can not be empty."); }
            List<Sponsorship> sponsorships = await _repo.GetAllByInstitutionAsync(institutionId);
            List<SponsorshipCard> cards = sponsorships.Select(sponsor => sponsor.GetCard()).ToList();
            
            return Result<List<SponsorshipCard>>.Success(cards);
        }

        public async Task<Result<Sponsorship>> GetByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code)) { return Result<Sponsorship>.Failure("Code cannot be empty."); }
            Sponsorship? sponsorInstance = await _repo.GetByCodeAsync(code);
            if (sponsorInstance is null) { return Result<Sponsorship>.Failure("Sponsorship not Found."); }

            return Result<Sponsorship>.Success(sponsorInstance);
        }

        public async Task<Result<DTSponsorship>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTSponsorship>.Failure("Sponsorship can not be empty."); }
            Sponsorship? sponsorInstance = await _repo.GetByIdAsync(id);
            if (sponsorInstance is null) { return Result<DTSponsorship>.Failure("Sponsorship not Found."); }

            List<string> errors = [];
            Result<Edition> editionResult = await _editionService.GetByIdAsync(sponsorInstance.Edition);
            if (!editionResult.IsSuccess) { errors.AddRange(editionResult.Errors); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(sponsorInstance.Institution);
            if (!institutionResult.IsSuccess) { errors.AddRange(institutionResult.Errors); }

            if (errors.Any()) { return Result<DTSponsorship>.Failure(errors); }

            return Result<DTSponsorship>.Success(sponsorInstance.GetDT(editionResult.Value!, institutionResult.Value!));
        }
    }
}
