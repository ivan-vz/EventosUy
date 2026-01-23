namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTClient(
        Guid id, 
        string nickname, 
        string email, 
        string firstName, 
        string? lastName, 
        string firstSurname,
        string lastSurname, 
        DateOnly birthday, 
        DateTimeOffset created,
        string ci
        )
    {
        public Guid Id { get; init; } = id;
        public string Nickname { get; init; } = nickname;
        public string Email { get; init; } = email;
        public string FirstName { get; init; } = firstName;
        public string? LastName { get; init; } = lastName;
        public string FirstSurname { get; init; } = firstSurname;
        public string LastSurname { get; init; } = lastSurname;
        public string Ci { get; init; } = ci;
        public DateOnly Birthday { get; init; } = birthday;
        public DateTimeOffset Created { get; init; } = created;
    }
}
