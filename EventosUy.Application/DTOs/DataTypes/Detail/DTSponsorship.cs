using EventosUy.Application.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTSponsorship(
        Guid id, 
        string name, 
        decimal amount, 
        SponsorshipTier tier, 
        DateTimeOffset created, 
        EditionCard editionCard, 
        UserCard institutionCard,
        RegisterTypeCard registerTypeCard
        )
    {
        public Guid Id { get; init; } = id;
        public string Name { get; init; } = name;
        public decimal Amount { get; init; } = amount;
        public SponsorshipTier Tier { get; init; } = tier;
        public DateTimeOffset Created { get; init; } = created;
        public EditionCard Edition { get; init; } = editionCard;
        public UserCard Institution { get; init; } = institutionCard;
        public RegisterTypeCard RegisterType { get; init; } = registerTypeCard;
    }
}
