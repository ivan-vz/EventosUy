using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using FluentValidation.Results;

namespace EventosUy.Application.Services
{
    public class SponsorshipService : ISponsorshipService
    {
        private readonly ISponsorshipRepo _repo;
        private readonly IEditionService _editionService;
        private readonly IInstitutionService _institutionService;
        private readonly IRegisterTypeService _registerTypeService;

        private static readonly Dictionary<SponsorshipTier, (decimal min, decimal max)> tierRanges = new() {
            { SponsorshipTier.BRONZE, (min: 1_000m, max: 9_999.99m) },
            { SponsorshipTier.SILVER, (min: 10_000m, max: 99_999.99m) },
            { SponsorshipTier.GOLD, (min: 100_000m, max: 999_999.99m) },
            { SponsorshipTier.PLATINUM, (min: 1_000_000m, max: decimal.MaxValue) },
        };

        public SponsorshipService(
            ISponsorshipRepo sponsorship, 
            IEditionService editionService, 
            IInstitutionService institutionService, 
            IRegisterTypeService registerTypeService,
            IVoucherService voucherService
            ) 
        {
            _repo = sponsorship;
            _editionService = editionService;
            _institutionService = institutionService;
            _registerTypeService = registerTypeService;
        }

        public async Task<(DTSponsorship? dt, ValidationResult validation)> CreateAsync(DTInsertSponsorship dtInsert)
        {
            var validationResult = new ValidationResult();

            var userCard = (await _institutionService.GetByIdAsync(dtInsert.Institution)).card;
            if (userCard is null) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Institution", "Institution not found.")
                    );
            }

            var editionCard = (await _editionService.GetByIdAsync(dtInsert.Edition)).card;
            if (editionCard is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition", "Edition not found.")
                    );
            }

            var registerTypeCard = (await _registerTypeService.GetByIdAsync(dtInsert.RegisterType)).card;
            if (registerTypeCard is null)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Register Type", "Register type not found.")
                    );
            }

            if (!tierRanges.TryGetValue(dtInsert.Tier, out var ranges))
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Tier", "Tier not found.")
                    );
            }

            if (dtInsert.Amount < ranges.min)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Tier", $"Amount must be at least {ranges.min:N0} for {dtInsert.Tier} tier.")
                    );
            }

            if (dtInsert.Amount > ranges.max)
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Tier", $"Amount {dtInsert.Amount:N0} exceeds maximum for {dtInsert.Tier} tier. Please upgrade to the next tier.")
                    );
            }

            if (await _repo.ExistsAsync(dtInsert.Edition, dtInsert.Institution)) 
            {
                validationResult.Errors.Add
                    (
                        new ValidationFailure("Edition | Institution", "Sponsorship already exists.")
                    );
            }

            if (!validationResult.IsValid) { return (null, validationResult); }
            
            var sponsorship = new Sponsorship(
                name: dtInsert.Name, 
                amount: dtInsert.Amount, 
                tier: dtInsert.Tier, 
                edition: dtInsert.Edition, 
                institution: dtInsert.Institution,
                registerType: dtInsert.RegisterType
                );

            await _repo.AddAsync(sponsorship);
            
            var dt = new DTSponsorship(
                    id: sponsorship.Id,
                    name: sponsorship.Name,
                    amount: sponsorship.Amount,
                    tier: sponsorship.Tier,
                    created: sponsorship.Created,
                    editionCard: editionCard!,
                    institutionCard: userCard!,
                    registerTypeCard: registerTypeCard!
                );

            return (dt, validationResult);
        }

        public async Task<IEnumerable<SponsorshipCard>> GetAllByEditionAsync(Guid editionId)
        {
            List<Sponsorship> sponsorships = await _repo.GetAllByEditionAsync(editionId);
            List<SponsorshipCard> cards = [.. sponsorships.Select(sponsor => new SponsorshipCard(sponsor.Id, sponsor.Name, sponsor.Tier) )];

            return cards;
        }

        public async Task<IEnumerable<SponsorshipCard>> GetAllByInstitutionAsync(Guid institutionId)
        {
            List<Sponsorship> sponsorships = await _repo.GetAllByInstitutionAsync(institutionId);
            List<SponsorshipCard> cards = [.. sponsorships.Select(sponsor => new SponsorshipCard(sponsor.Id, sponsor.Name, sponsor.Tier))];
            
            return cards;
        }

        public async Task<(DTSponsorship? dt, SponsorshipCard? card)> GetByIdAsync(Guid id)
        {
            Sponsorship? sponsor = await _repo.GetByIdAsync(id);
            if (sponsor is null) { return (null, null); }

            var userCard = (await _institutionService.GetByIdAsync(sponsor.Institution)).card;
            var editionCard = (await _editionService.GetByIdAsync(sponsor.Edition)).card;
            var registerTypeCard = (await _registerTypeService.GetByIdAsync(sponsor.RegisterType)).card;

            var dt = new DTSponsorship(
                    id: sponsor.Id,
                    name: sponsor.Name,
                    amount: sponsor.Amount,
                    tier: sponsor.Tier,
                    created: sponsor.Created,
                    editionCard: editionCard!,
                    institutionCard: userCard!,
                    registerTypeCard: registerTypeCard!
                );

            var card = new SponsorshipCard(Id: sponsor.Id, Name: sponsor.Name, Tier: sponsor.Tier);

            return (dt, card);
        }

        public async Task<DTSponsorship?> DeleteAsync(Guid id)
        {
            var sponsor = await _repo.GetByIdAsync(id);

            if (sponsor is null) { return null; }

            sponsor.Active = false;

            var userCard = (await _institutionService.GetByIdAsync(sponsor.Institution)).card;
            var editionCard = (await _editionService.GetByIdAsync(sponsor.Edition)).card;
            var registerTypeCard = (await _registerTypeService.GetByIdAsync(sponsor.RegisterType)).card;

            var dt = new DTSponsorship(
                    id: sponsor.Id,
                    name: sponsor.Name,
                    amount: sponsor.Amount,
                    tier: sponsor.Tier,
                    created: sponsor.Created,
                    editionCard: editionCard!,
                    institutionCard: userCard!,
                    registerTypeCard: registerTypeCard!
                );

            return dt;
        }

        public async Task<bool> ValidateCodeAsync(string code, Guid editionId, Guid registerTypeId) => await _repo.ValidateCodeAsync(code, editionId, registerTypeId);
    }
}
