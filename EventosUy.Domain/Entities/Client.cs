using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Domain.Entities
{
    public class Client : User
    {
        public Name Name { get; init; }
        public DateOnly Birthday { get; init; }
        public Ci CI { get; init; }

        private Client(string nickname, Password password, Email email, Name name, DateOnly birthday, Ci ci) 
            : base(nickname, password, email)
        {
            Name = name;
            Birthday = birthday;
            CI = ci;
        }

        public static Result<Client> Create(string nickname, Password password, Email email, Name name, DateOnly birthday, Ci ci) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(nickname)) { errors.Add("Nickname can not be empty."); }
            if (birthday >= DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Invalid Birthday's date."); }

            if (errors.Count != 0) { return Result<Client>.Failure(errors); }

            Client clientInstance = new(nickname, password, email, name, birthday, ci);

            return Result<Client>.Success(clientInstance);
        }

        public DTClient GetDT() { return new(Nickname, Email.Value, Name.FullName, Birthday, Created, CI.GetFormatted()); }

        public UserCard GetCard() { return new(Id, Nickname, Email.Value); }
    }
}
