using EventosUy.Domain.Common;

namespace EventosUy.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; init; }

        private Email(string value) { Value = value; }

        public static Result<Email> Create(string value) 
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@')) { return Result<Email>.Failure("Email is not formatted correctly."); }

            Email email = new Email(value);

            return Result<Email>.Success(email);
        }
    }
}
