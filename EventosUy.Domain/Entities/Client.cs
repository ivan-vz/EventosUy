namespace EventosUy.Domain.Entities
{
    public class Client(
        string nickname,
        string password,
        string email,
        string firstName,
        string? lastName,
        string firstSurname,
        string lastSurname,
        DateOnly birthday,
        string ci
            ) : User(nickname, password, email)
    {
        public string FirstName { get; init; } = firstName;
        public string? LastName { get; init; } = lastName;
        public string FirstSurname { get; init; } = firstSurname;
        public string LastSurname { get; init; } = lastSurname;
        public DateOnly Birthday { get; init; } = birthday;
        public string Ci { get; init; } = ci;
    }
}
