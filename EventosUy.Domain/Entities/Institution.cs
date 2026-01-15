using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Institution : User
    {
        public string Name { get; private set; }
        public string Acronym { get; private set; }
        public string Description { get; private set; }
        public Address Address { get; private set; }
        public Url Url { get; private set; }

        private Institution(string acronym, string description, Address address, Url url, string nickname, Password password, string name, Email email) 
            : base(nickname, password, email)
        {
            Name = name;
            Acronym = acronym;
            Description = description;
            Address = address;
            Url = url;
        }

        public static Result<Institution> Create(string nickname, string acronym, Password password, Email email, string name, Url url, Address address, string description) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(nickname)) { errors.Add("Nickname cannot be empty."); }
            if (string.IsNullOrWhiteSpace(name)) { errors.Add("Name cannot be empty."); }
            if (string.IsNullOrWhiteSpace(description)) { errors.Add("Description cannot be empty."); }
            if (string.IsNullOrWhiteSpace(acronym)) { errors.Add("Acronym cannot be empty."); }
            if (acronym.Length < 2 || acronym.Length > 10) { errors.Add("Acronym cannot must have between 2 and 10 characters."); }
            if (acronym.Any(c => !char.IsLetter(c))) { errors.Add("Acronyms can only contain letters."); }

            if (errors.Count != 0) { return Result<Institution>.Failure(errors); }

            Institution institutionInstance = new(acronym, description, address, url, nickname, password, name, email);

            return Result<Institution>.Success(institutionInstance);
        }

        public DTInsitution GetDT() { return new(Nickname, Email.Value, Name, Description, Url.Value, Address.FullAddress, Created); }

        public UserCard GetCard() { return new(Id, Nickname, Email.Value); }
    }
}
