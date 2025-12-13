using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;
using System.Runtime.InteropServices;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTimeOffset Created { get; init; }
        public float Amount { get; init; }
        public int Free { get; init; }
        public int Used { get; private set; }
        public string Code { get; init; }
        public SponsorshipTier Tier { get; init; }
        public SponsorshipState State { get; private set; }
        public DateOnly Expired { get; private set; }
        public Guid Institution { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        private Sponsorship(string name, float amount, int free, string code, SponsorshipTier tier, Guid institutionId, Guid editionId, Guid registerTypeId, DateOnly expired)
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTimeOffset.UtcNow;
            Amount = amount;
            Free = free;
            Used = 0;
            Code = code;
            Tier = tier;
            Institution = institutionId;
            Edition = editionId;
            RegisterType = registerTypeId;
            State = SponsorshipState.AVAILABLE;
            Expired = expired;
        }

        public static Result<Sponsorship> Create(
            string name, float amount, int free, string code, SponsorshipTier tier, Institution institutionInstance, Edition editionInstance, RegisterType registerTypeInstance, DateOnly expired
            ) 
        {
            if (institutionInstance is null) { return Result<Sponsorship>.Failure("Institution can not be empty."); }
            if (editionInstance is null) { return Result<Sponsorship>.Failure("Edition can not be empty."); }
            if (registerTypeInstance is null) { return Result<Sponsorship>.Failure("Register type can not be empty."); }

            if (string.IsNullOrWhiteSpace(name)) { return Result<Sponsorship>.Failure("Name can not be empty."); }
            if (string.IsNullOrWhiteSpace(code)) { return Result<Sponsorship>.Failure("Code can not be empty."); }

            if (amount <= 0) { return Result<Sponsorship>.Failure("Amount must be greater than 0."); }
            if (free < 0) { return Result<Sponsorship>.Failure("Amountmust be greater than or equal to 0."); }
            if (free * registerTypeInstance.Price > 0.2 * amount) { return Result<Sponsorship>.Failure("The value of free registrations may not exceed 20% of the institution's contribution."); }

            if (expired <= DateOnly.FromDateTime(DateTime.UtcNow)) { return Result<Sponsorship>.Failure("Expiration date must be after today's date."); }

            Sponsorship sponsorshipInstance = new Sponsorship(name, amount, free, code, tier, institutionInstance.Id, editionInstance.Id, registerTypeInstance.Id, expired );

            return Result<Sponsorship>.Success(sponsorshipInstance);
        }

        public DTSponsorship GetDT(Edition editionInstance, Institution institutionInstance) 
        { 
            return new DTSponsorship(Name, Created, Expired, Amount, Free, Code, Tier, editionInstance.Name, institutionInstance.Nickname); 
        }

        public SponsorshipCard GetCard() { return new SponsorshipCard(Id, Name, Expired, Tier); }
    }
}
