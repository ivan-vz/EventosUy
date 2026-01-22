namespace EventosUy.Application.DTOs.DataTypes.Update
{
    public class DTUpdateInstitution(
        Guid id,
        string nickname,
        string password,
        string email,
        string description,
        string url
        )
    {
        public Guid Id { get; init; } = id;
        public string Nickname { get; init; } = nickname;
        public string Password { get; init; } = password;
        public string Email { get; init; } = email;
        public string Description { get; init; } = description;
        public string Url { get; init; } = url;
    }
}
