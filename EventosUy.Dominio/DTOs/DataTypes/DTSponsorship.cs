using EventosUy.Dominio.Enumerados;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTSponsorship
    {
        public string Name { get; init; }
        public DateOnly Created { get; init; }
        public DateOnly Expired { get; init; }
        public float Amount { get; init; }
        public int Free { get; init; }
        public string Code { get; init; }
        public SponsorshipTier Tier { get; init; }
        public string Edition { get; init; }
        public string Institution { get; init; }

        public DTSponsorship(string name, DateOnly created, DateOnly expired, float amount, int free, string code, SponsorshipTier tier, string edition, string institution) 
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
