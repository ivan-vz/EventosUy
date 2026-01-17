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
            if (birthday >= DateOnly.FromDateTime(DateTime.UtcNow)) { return Result<Client>.Failure("Invalid Birthday's date."); }

            Client clientInstance = new(nickname, password, email, name, birthday, ci);

            return Result<Client>.Success(clientInstance);
        }

        public DTClient GetDT() { return new(Nickname, Email.Value, Name.FullName, Birthday, Created, CI.GetFormatted()); }

        public UserCard GetCard() { return new(Id, Nickname, Email.Value); }
    }
}
