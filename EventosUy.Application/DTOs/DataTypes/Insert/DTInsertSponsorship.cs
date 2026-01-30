using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertSponsorship(string name, SponsorshipTier tier, decimal amount, Guid institutionId, Guid registerTypeId, string voucherName, string voucherCode)
    {
        public string Name { get; init; } = name;
        public SponsorshipTier Tier { get; init; } = tier;
        public decimal Amount { get; init; } = amount;
        public Guid Institution { get; init;} = institutionId;
        public Guid RegisterType { get; init; } = registerTypeId;
        public string VoucherName { get; init; } = voucherName;
        public string VoucherCode { get; init; } = voucherCode;
    }
}
