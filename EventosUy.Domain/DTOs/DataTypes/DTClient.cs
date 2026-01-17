namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTClient(string nickname, string email, string fullName, DateOnly birthday, DateTimeOffset created, string ci)
    {
        public string Nickname { get; init; } = nickname;
        public string Email { get; init; } = email;
        public string FullName { get; init; } = fullName;
        public string Ci { get; init; } = ci;
        public DateOnly Birthday { get; init; } = birthday;
        public DateTimeOffset Created { get; init; } = created;
    }
}
