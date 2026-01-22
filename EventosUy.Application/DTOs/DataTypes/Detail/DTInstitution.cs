namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTInstitution(string nickname, string email, string name, string acronym, string description, string url, string address, DateTimeOffset created)
    {
        public string Nickname { get; init; } = nickname;
        public string Email { get; init; } = email;
        public string Name { get; init; } = name;
        public string Acronym { get; init; } = acronym;
        public string Description { get; init; } = description;
        public string Url { get; init; } = url;
        public string Address { get; init; } = address;
        public DateTimeOffset Created { get; init; } = created;
    }
}
