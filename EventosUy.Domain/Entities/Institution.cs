using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Institution : User
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Address Address { get; private set; }
        public Url Url { get; private set; }

        private Institution(string description, Address address, Url url, string nickname, Password password, string name, Email email) 
            : base(nickname, password, email)
        {
            Name = name;
            Description = description;
            Address = address;
            Url = url;
        }

        public static Result<Institution> Create(string nickname, Password password, Email email, string name, Url url, Address address, string description) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(nickname)) { errors.Add("Nickname cannot be empty."); }
            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name cannot be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { errors.Add("Description cannot be empty."); }

            if (errors.Any()) { return Result<Institution>.Failure(errors); }

            Institution institutionInstance = new Institution(description, address, url, nickname, password, name, email);

            return Result<Institution>.Success(institutionInstance);
        }

        public DTInsitution GetDT() { return new DTInsitution(Nickname, Email.Value, Name, Description, Url.Value, Address.FullAddress, Created); }

        public ProfileCard GetCard() { return new ProfileCard(Id, Nickname, Email.Value); }
    }
}
