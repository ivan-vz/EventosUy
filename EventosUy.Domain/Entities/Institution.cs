using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Institution : User
    {
        public string Description { get; private set; }
        public Address Address { get; private set; }
        public Url Url { get; private set; }

        public Institution(string description, Address address, Url url, string nickname, string password, string name, Email email) 
            : base(nickname, password, name, email)
        {
            Description = description;
            Address = address;
            Url = url;
        }

        public DTInsitution GetDT() { return new DTInsitution(Nickname, Email.Value, Name, Description, Url.Value, Created); }

        public ProfileCard GetCard() { return new ProfileCard(Id, Nickname, Email.Value); }
    }
}
