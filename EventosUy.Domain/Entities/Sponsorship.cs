using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;
using System.Runtime.InteropServices;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTimeOffset Created { get; init; }
        public SponsorLevel Level { get; init; }
        public int Free { get; init; }
        public int Used { get; private set; }
        public string Code { get; init; }
        public SponsorshipState State { get; private set; }
        public DateOnly Expired { get; private set; }
        public Guid Institution { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        private Sponsorship(string name, int free, string code, SponsorLevel level, Guid institutionId, Guid editionId, Guid registerTypeId, DateOnly expired)
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTimeOffset.UtcNow;
            Free = free;
            Used = 0;
            Code = code;
            Level = level;
            Institution = institutionId;
            Edition = editionId;
            RegisterType = registerTypeId;
            State = SponsorshipState.AVAILABLE;
            Expired = expired;
        }

        public static Result<Sponsorship> Create(
            string name, int free, string code, SponsorLevel level, Institution institutionInstance, Edition editionInstance, RegisterType registerTypeInstance, DateOnly expired
            ) 
        {
            List<string> errors = [];
            if (institutionInstance is null) { errors.Add("Institution cannot be empty."); }
            if (editionInstance is null) { errors.Add("Edition cannot be empty."); }
            if (registerTypeInstance is null) { errors.Add("Register type cannot be empty."); }

            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name cannot be empty."); }
            if (string.IsNullOrWhiteSpace(code)) { errors.Add("Code cannot be empty."); }

            if (free < 0) { errors.Add("Amount must be greater than or equal to 0."); }
            
            if (expired <= DateOnly.FromDateTime(DateTime.UtcNow)) { return Result<Sponsorship>.Failure("Expiration date must be after today's date."); }
            
            if (errors.Any()) { return Result<Sponsorship>.Failure(errors); }

            if (free * registerTypeInstance!.Price > 0.2 * level.Amount) { return Result<Sponsorship>.Failure("The value of free registrations may not exceed 20% of the institution's contribution."); }

            Sponsorship sponsorshipInstance = new Sponsorship(name, free, code, level, institutionInstance!.Id, editionInstance!.Id, registerTypeInstance!.Id, expired);

            return Result<Sponsorship>.Success(sponsorshipInstance);
        }

        public DTSponsorship GetDT(Edition editionInstance, Institution institutionInstance) 
        { 
            return new DTSponsorship(Name, Created, Expired, Level.Amount, Free, Code, Level.Tier, editionInstance.Name, institutionInstance.Nickname); 
        }

        public SponsorshipCard GetCard() { return new SponsorshipCard(Id, Name, Expired, Level.Tier); }
    }
}
