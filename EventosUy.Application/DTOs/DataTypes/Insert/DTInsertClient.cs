namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertClient(
        string nickname, 
        string password, 
        string email, 
        DateOnly birthday, 
        string ci, 
        string firstName, 
        string? lastName, 
        string firstSurname, 
        string lastSurname
        )
    {
        public string Nickname { get; init; } = nickname;
        public string Password { get; init; } = password;
        public string Email { get; init; } = email;
        public DateOnly Birthday { get; init; } = birthday;
        public string Ci { get; init; } = ci;
        public string FirstName { get; init; } = firstName;
        public string? LastName { get; init; } = lastName;
        public string FirstSurname { get; init; } = firstSurname;
        public string LastSurname { get; init; } = lastSurname;
    }
}
