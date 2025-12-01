namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTInsitution
    {
        public string Nickname { get; init; }
        public string Email { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Url { get; init; }
        public DateTimeOffset Created { get; init; }

        public DTInsitution(string nickname, string email, string name, string description, string url, DateTimeOffset created)
        {
            Nickname = nickname;
            Email = email;
            Name = name;
            Description = description;
            Url = url;
            Created = created;
        }
    }
}
