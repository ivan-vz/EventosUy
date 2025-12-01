namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTPerson
    {
        public string Nickname { get; init; }
        public string Email { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateOnly Birthday { get; init; }
        public DateOnly Created { get; init; }

        public DTPerson(string nickname, string email, string name, string surname, DateOnly birthday, DateOnly created) 
        {
            Nickname = nickname;
            Email = email;
            Name = name;
            Surname = surname;
            Birthday = birthday;
            Created = created;
        }
    }
}
