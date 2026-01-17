using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTSponsorship(string name, decimal amount, SponsorshipTier tier, DateTimeOffset created, ActivityCard editionCard, UserCard institutionCard, VoucherCard voucherCard)
    {
        public string Name { get; init; } = name;
        public decimal Amount { get; init; } = amount;
        public SponsorshipTier Tier { get; init; } = tier;
        public DateTimeOffset Created { get; init; } = created;
        public ActivityCard Edition { get; init; } = editionCard;
        public UserCard Institution { get; init; } = institutionCard;
        public VoucherCard Voucher { get; init; } = voucherCard;
    }
}
