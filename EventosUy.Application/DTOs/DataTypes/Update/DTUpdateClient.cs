namespace EventosUy.Application.DTOs.DataTypes.Update
{
    public class DTUpdateClient(Guid id, string nickname, string password, string email)
    {
        public Guid Id { get; init; } = id;
        public string Nickname { get; init; } = nickname;
        public string Password { get; init; } = password;
        public string Email { get; init; } = email;
    }
}
