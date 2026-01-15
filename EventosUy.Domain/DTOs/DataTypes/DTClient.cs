namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTClient
    {
        public string Nickname { get; init; }
        public string Email { get; init; }
        public string FullName { get; init; }
        public string Ci { get; init; }
        public DateOnly Birthday { get; init; }
        public DateTimeOffset Created { get; init; }

        public DTClient(string nickname, string email, string fullName, DateOnly birthday, DateTimeOffset created, string ci) 
        {
            Nickname = nickname;
            Email = email;
            FullName = fullName;
            Ci = ci;
            Birthday = birthday;
            Created = created;
        }
    }
}
