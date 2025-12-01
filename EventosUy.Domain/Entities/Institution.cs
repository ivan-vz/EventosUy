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
    }
}
