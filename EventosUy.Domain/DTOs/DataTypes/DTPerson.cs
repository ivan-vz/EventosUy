namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTPerson
    {
        public string Nickname { get; init; }
        public string Email { get; init; }
        public string FullName { get; init; }
        public DateOnly Birthday { get; init; }
        public DateTimeOffset Created { get; init; }

        public DTPerson(string nickname, string email, string fullName, DateOnly birthday, DateTimeOffset created) 
        {
            Nickname = nickname;
            Email = email;
            FullName = fullName;
            Birthday = birthday;
            Created = created;
        }
    }
}
