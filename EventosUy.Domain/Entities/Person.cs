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
    }
}
