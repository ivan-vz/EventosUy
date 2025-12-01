using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; init; }
        public string Nickname { get; private set; }
        public string Password { get; private set; }
        public string Name { get; init; }
        public Email Email { get; private set; }
        public DateTimeOffset Created { get; init; }
        public UserState State { get; set; }

        protected User(string nickname, string password, string name, Email email) 
        {
            Id = Guid.NewGuid();
            Nickname = nickname;
            Password = password;
            Name = name;
            Email = email;
            Created = DateTimeOffset.UtcNow;
            State = UserState.ACTIVE;
        }
    }
}
