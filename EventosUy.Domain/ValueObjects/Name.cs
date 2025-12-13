using EventosUy.Domain.Common;

namespace EventosUy.Domain.ValueObjects
{
    public record Name
    {
        public string FirstName { get; }
        public string? LastName { get; }
        public string FirstSurname { get; }
        public string LastSurname { get; }
        public string FullName { get; }

        private Name(string firstSurname, string lastSurname, string firstName, string? lastName = null) 
        {
            FirstSurname = firstSurname;
            LastSurname = lastSurname;
            FirstName = firstName;
            LastName = lastName;

            FullName = (lastName is null) ? $"{firstName} {firstSurname} {lastSurname}" : $"{firstName} {lastName} {firstSurname} {lastSurname}";
        }

        public static Result<Name> Create(string firstSurname, string lastSurname, string firstName, string? lastName = null) 
        {
            List<String> errors = [];
            if (string.IsNullOrWhiteSpace(firstSurname)) { errors.Add("First surname cannot be empty."); }
            if (string.IsNullOrWhiteSpace(lastSurname)) { errors.Add("Last surname cannot be empty."); }
            if (string.IsNullOrWhiteSpace(firstName)) { errors.Add("First name cannot be empty."); }

            if (errors.Any()) { return Result<Name>.Failure(errors); }

            if (firstSurname.Any(c => !char.IsLetter(c))) { errors.Add("First surname cannot have anythings else than letters."); }
            if (lastSurname.Any(c => !char.IsLetter(c))) { errors.Add("Last surname cannot have anythings else than letters."); }
            if (firstName.Any(c => !char.IsLetter(c))) { errors.Add("First name cannot have anythings else than letters."); }
            if (lastName is not null && lastName.Any(c => !char.IsLetter(c))) { errors.Add("Last name cannot have anythings else than letters."); }

            if (errors.Any()) { return Result<Name>.Failure(errors); }

            Name name = new Name(firstSurname, lastSurname, firstName, lastName);

            return Result<Name>.Success(name);
        }
    }
}
