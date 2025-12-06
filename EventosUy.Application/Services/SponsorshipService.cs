using EventosUy.Application.Interfaces;
using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using System.Xml.Linq;

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

        public async Task<Result<Guid>> CreateAsync(string name, SponsorshipTier tier, float amount, int free, string code, DateOnly expiration, Guid editionId, Guid institutionId, Guid registerTypeId)
        {
            Result<Edition> editionResult = await _editionService.GetByIdAsync(editionId);
            if (!editionResult.IsSuccess) { return Result<Guid>.Failure(editionResult.Error!); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(institutionId);
            if (!institutionResult.IsSuccess) { return Result<Guid>.Failure(institutionResult.Error!); }

            Result<RegisterType> registerTypeResult = await _registerTypeService.GetByIdAsync(registerTypeId);
            if (!registerTypeResult.IsSuccess) { return Result<Guid>.Failure(registerTypeResult.Error!); }

            if (await _repo.ExistsAsync(editionId, institutionId)) { return Result<Guid>.Failure("Sponsorship already exist."); }

            Result<Sponsorship> sponsorResult = Sponsorship.Create(name, amount, free, code, tier, institutionResult.Value!, editionResult.Value!, registerTypeResult.Value!, expiration);
            if (!sponsorResult.IsSuccess) { return Result<Guid>.Failure(sponsorResult.Error!); }

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

        public async Task<Result<DTSponsorship>> GetDTAsync(Guid id)
        {
            if (id == Guid.Empty) { return Result<DTSponsorship>.Failure("Sponsorship can not be empty."); }
            Sponsorship? sponsorInstance = await _repo.GetByIdAsync(id);
            if (sponsorInstance is null) { return Result<DTSponsorship>.Failure("Sponsorship not Found."); }

            Result<Edition> editionResult = await _editionService.GetByIdAsync(sponsorInstance.Edition);
            if (!editionResult.IsSuccess) { return Result<DTSponsorship>.Failure(editionResult.Error!); }

            Result<Institution> institutionResult = await _institutionService.GetByIdAsync(sponsorInstance.Institution);
            if (!institutionResult.IsSuccess) { return Result<DTSponsorship>.Failure(institutionResult.Error!); }

            return Result<DTSponsorship>.Success(sponsorInstance.GetDT(editionResult.Value!, institutionResult.Value!));
        }

        public async Task<Result> ValidateCodeAsync(Guid registerTypeId, string sponsorCode) 
        {
            if (registerTypeId == Guid.Empty) { return Result.Failure("Register Type can not be empty."); }
            if (string.IsNullOrWhiteSpace(sponsorCode)) { return Result.Failure("Sponsor Code can not be empty."); }

            if (!(await _repo.ValidateCodeAsync(registerTypeId, sponsorCode))) { return Result.Failure("Invalid code."); }

            return Result.Success();
        }
    }
}
