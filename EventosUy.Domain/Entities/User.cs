using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; init; }
        public string Nickname { get; private set; }
        public Password Password { get; private set; }
        public Email Email { get; private set; }
        public DateTimeOffset Created { get; init; }
        public UserState State { get; set; }

        protected User(string nickname, Password password, Email email) 
        {
            Id = Guid.NewGuid();
            Nickname = nickname;
            Password = password;
            Email = email;
            Created = DateTimeOffset.UtcNow;
            State = UserState.ACTIVE;
        }
    }
}
