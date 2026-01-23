namespace EventosUy.Domain.Entities
{
    public abstract class User(string nickname, string password, string email)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Nickname { get; set; } = nickname;
        public string Password { get; set; } = password;
        public string Email { get; set; } = email;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
        public bool Active { get; set; } = true;
    }
}
