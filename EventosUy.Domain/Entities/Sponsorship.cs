using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Amount { get; init; }
        public SponsorshipTier Tier { get; init; }
        public DateTimeOffset Created { get; init; }
        public Guid Edition { get; init; }
        public Guid Institution { get; init; }
        public Guid RegisterType { get; init; }
        public Guid Voucher { get; private set; }

        private Sponsorship(string name, decimal amount, SponsorshipTier tier, Guid editon, Guid institution, Guid registerType)
        {
            Id = Guid.NewGuid();
            Name = name;
            Amount = amount;
            Tier = tier;
            Created = DateTimeOffset.UtcNow;
            Edition = editon;
            Institution = institution;
            RegisterType = registerType;
            Voucher = Guid.Empty;
        }

        private static readonly Dictionary<SponsorshipTier, (decimal min, decimal max)> tierRanges = new() {
            { SponsorshipTier.BRONZE, (min: 1_000m, max: 9_999.99m) },
            { SponsorshipTier.SILVER, (min: 10_000m, max: 99_999.99m) },
            { SponsorshipTier.GOLD, (min: 100_000m, max: 999_999.99m) },
            { SponsorshipTier.PLATINUM, (min: 1_000_000m, max: decimal.MaxValue) },
        };

        public static Result<Sponsorship> Create(
            string name, decimal amount, SponsorshipTier tier, Institution institution, Edition edition, RegisterType registerType
            ) 
        {
            List<string> errors = [];
            if (!tierRanges.TryGetValue(tier, out var ranges))
            {
                errors.Add($"Tier {tier} not Found.");
                return Result<Sponsorship>.Failure(errors);
            }

            if (amount < ranges.min) { errors.Add($"Amount must be at least {ranges.min:N0} for {tier} tier."); }
            if (amount > ranges.max) { errors.Add($"Amount {amount:N0} exceeds maximum for {tier} tier. Please upgrade to the next tier."); }
            if (registerType.Price < 0) { errors.Add("Register type price must be greater than or equal to 0."); }

            if (errors.Count != 0) { return Result<Sponsorship>.Failure(errors); }

            Sponsorship sponsorshipInstance = new(name, amount, tier, edition.Id, institution.Id, registerType.Id);

            // TODO: De esto se encarga SponsorshipService
            //int free = (int)Math.Floor((0.2m * amount) / registerTypePrice);

            return Result<Sponsorship>.Success(sponsorshipInstance);
        }

        public DTSponsorship GetDT(Edition edition, Institution institution, Voucher voucher)
        {
            return new(Name, Amount, Tier, Created, edition.GetCard(), institution.GetCard(), voucher.GetCard()); 
        }

        public SponsorshipCard GetCard() { return new(Id, Name, Tier); }

        public Result AssignVoucher(Guid voucher) 
        {
            if (Voucher != Guid.Empty) { return Result.Failure("Voucher already assigned."); }

            Voucher = voucher;

            return Result.Success();
        }
    }
}
