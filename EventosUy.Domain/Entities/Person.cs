using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Person : User
    {
        public Name Name { get; init; }
        public DateOnly Birthday { get; init; }

        private Person(string nickname, Password password, Email email, Name name, DateOnly birthday) 
            : base(nickname, password, email)
        {
            Name = name;
            Birthday = birthday;
        }

        public static Result<Person> Create(string nickname, Password password, Email email, Name name, DateOnly birthday) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(nickname)) { errors.Add("Nickname can not be empty."); }
            if (birthday >= DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Invalid Birthday's date."); }

            if (errors.Any()) { return Result<Person>.Failure(errors); }

            Person personInstance = new Person(nickname, password, email, name, birthday);

            return Result<Person>.Success(personInstance);
        }

        public DTPerson GetDT() { return new DTPerson(Nickname, Email.Value, Name.FullName, Birthday, Created); }

        public ProfileCard GetCard() { return new ProfileCard(Id, Nickname, Email.Value); }
    }
}
