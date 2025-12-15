using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;
using System;

namespace EventosUy.Domain.Entities
{
    public class Sponsorship
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTimeOffset Created { get; init; }
        public SponsorLevel Level { get; init; }
        public int Used { get; private set; }
        public string Code { get; init; }
        public SponsorshipState State { get; private set; }
        public DateOnly Expired { get; private set; }
        public Guid Institution { get; init; }
        public Guid Edition { get; init; }
        public Guid RegisterType { get; init; }

        private Sponsorship(string name, string code, SponsorLevel level, Guid institutionId, Guid editionId, Guid registerTypeId, DateOnly expired)
        {
            Id = Guid.NewGuid();
            Name = name;
            Created = DateTimeOffset.UtcNow;
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
            string name, SponsorLevel level, Institution institutionInstance, Edition editionInstance, RegisterType registerTypeInstance, DateOnly expired
            ) 
        {
            List<string> errors = [];
            if (level is null) { errors.Add("Sponsor level cannot be empty."); }
            if (institutionInstance is null) { errors.Add("Institution cannot be empty."); }
            if (editionInstance is null) { errors.Add("Edition cannot be empty."); }
            if (registerTypeInstance is null) { errors.Add("Register type cannot be empty."); }

            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name cannot be empty."); }
            
            if (expired <= DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Expiration date must be after today's date."); }
            
            if (errors.Any()) { return Result<Sponsorship>.Failure(errors); }

            var code = $"#{institutionInstance!.Acronym}_{editionInstance!.Initials}_{editionInstance!.From.Year}";

            Sponsorship sponsorshipInstance = new Sponsorship(name, code, level!, institutionInstance!.Id, editionInstance!.Id, registerTypeInstance!.Id, expired);

            return Result<Sponsorship>.Success(sponsorshipInstance);
        }

        public DTSponsorship GetDT(Edition editionInstance, Institution institutionInstance) 
        { 
            return new DTSponsorship(Name, Created, Expired, Level.Amount, Level.FreeSpots, Code, Level.Tier, editionInstance.Name, institutionInstance.Nickname); 
        }

        public SponsorshipCard GetCard() { return new SponsorshipCard(Id, Name, Expired, Level.Tier); }

        public bool IsActive() { return State == SponsorshipState.AVAILABLE; }

        public Result UseSpot() 
        {
            if (Used >= Level.FreeSpots) { return Result.Failure("No avalable sponsor spots."); }

            Used++;

            if (Used == Level.FreeSpots) { State = SponsorshipState.COMPLETED; }
            
            return Result.Success();
        }
    }
}
