using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Person : User
    {
        public string Surname { get; init; }
        public DateOnly Birthday { get; init; }

        public Person(string surname, DateOnly birthday, string nickname, string password, string name, Email email) 
            : base(nickname, password, name, email)
        {
            Surname = surname;
            Birthday = birthday;
        }

        public DTPerson GetDT() { return new DTPerson(Nickname, Email.Value, Name, Surname, Birthday, Created); }

        public ProfileCard GetCard() { return new ProfileCard(Id, Nickname, Email.Value); }
    }
}
