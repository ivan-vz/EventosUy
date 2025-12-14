using EventosUy.Domain.Common;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.ValueObjects
{
    public record SponsorLevel
    {
        public float Amount { get; }
        public SponsorshipTier Tier { get; }

        private SponsorLevel(float amount, SponsorshipTier tier) 
        {
            Amount = amount;
            Tier = tier;
        }

        public static Result<SponsorLevel> Create(float amount, SponsorshipTier tier) 
        {
            List<string> errors = [];
            if (amount <= 0) { errors.Add("Amount must be greater than 0."); }
         
            var min = GetMinimumByTier(tier);
            if (amount < min) { errors.Add($"Amount must be at least {min} for {tier} tier."); }

            if (errors.Any()) { return Result<SponsorLevel>.Failure(errors); }

            var instance = new SponsorLevel(amount, tier);

            return Result<SponsorLevel>.Success(instance);
        }

        private static float GetMinimumByTier(SponsorshipTier tier) => tier switch
        {
            SponsorshipTier.BRONZE => 1_000,
            SponsorshipTier.SILVER => 10_000,
            SponsorshipTier.GOLD => 100_000,
            SponsorshipTier.PLATINUM => 1_000_000,
            _ => throw new ArgumentOutOfRangeException(nameof(tier), "Invalid tier")
        };
    }
}
