using EventosUy.Domain.Common;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.ValueObjects
{
    public record SponsorLevel
    {
        public decimal Amount { get; }
        public SponsorshipTier Tier { get; }
        public int FreeSpots { get; }

        private static readonly Dictionary<SponsorshipTier, (decimal min, decimal max)> tierRanges = new() {
            { SponsorshipTier.BRONZE, (min: 1_000m, max: 9_999.99m) },
            { SponsorshipTier.SILVER, (min: 10_000m, max: 99_999.99m) },
            { SponsorshipTier.GOLD, (min: 100_000m, max: 999_999.99m) },
            { SponsorshipTier.PLATINUM, (min: 1_000_000m, max: decimal.MaxValue) },
        };

        private SponsorLevel(decimal amount, SponsorshipTier tier, int free) 
        {
            Amount = amount;
            Tier = tier;
            FreeSpots = free;
        }

        public static Result<SponsorLevel> Create(decimal amount, SponsorshipTier tier, decimal registerTypePrice) 
        {
            List<string> errors = [];
            if (!tierRanges.TryGetValue(tier, out var ranges)) 
            { 
                errors.Add($"Tier {tier} not Found.");
                return Result<SponsorLevel>.Failure(errors);
            }
            
            if (amount < ranges.min) { errors.Add($"Amount must be at least {ranges.min:N0} for {tier} tier."); }
            if (amount > ranges.max) { errors.Add($"Amount {amount:N0} exceeds maximum for {tier} tier. Please upgrade to the next tier."); }
            if (registerTypePrice < 0) { errors.Add("Register type price must be greater than or equal to 0."); }

            if (errors.Count != 0) { return Result<SponsorLevel>.Failure(errors); }

            int free = (int)Math.Floor((0.2m * amount) / registerTypePrice);

            SponsorLevel instance = new(amount, tier, free);

            return Result<SponsorLevel>.Success(instance);
        }
    }
}