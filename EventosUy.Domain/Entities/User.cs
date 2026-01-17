using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public abstract class User(string nickname, Password password, Email email)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Nickname { get; private set; } = nickname;
        public Password Password { get; private set; } = password;
        public Email Email { get; private set; } = email;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public UserState State { get; set; } = UserState.ACTIVE;
    }
}
