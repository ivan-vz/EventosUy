using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTSponsorship
    {
        public string Name { get; init; }
        public DateTimeOffset Created { get; init; }
        public DateOnly Expired { get; init; }
        public decimal Amount { get; init; }
        public int Free { get; init; }
        public string Code { get; init; }
        public SponsorshipTier Tier { get; init; }
        public string Edition { get; init; }
        public string Institution { get; init; }

        public DTSponsorship(string name, DateTimeOffset created, DateOnly expired, decimal amount, int free, string code, SponsorshipTier tier, string edition, string institution) 
        {
            Name = name;
            Created = created;
            Expired = expired;
            Amount = amount;
            Free = free;
            Code = code;
            Tier = tier;
            Edition = edition;
            Institution = institution;
        }
    }
}
