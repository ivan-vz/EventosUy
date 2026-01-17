namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTInsitution(string nickname, string email, string name, string description, string url, string address, DateTimeOffset created)
    {
        public string Nickname { get; init; } = nickname;
        public string Email { get; init; } = email;
        public string Name { get; init; } = name;
        public string Description { get; init; } = description;
        public string Url { get; init; } = url;
        public string Address { get; init; } = address;
        public DateTimeOffset Created { get; init; } = created;
    }
}
