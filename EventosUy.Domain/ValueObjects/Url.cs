using EventosUy.Domain.Common;

namespace EventosUy.Domain.ValueObjects
{
    public record Url
    {
        public string Value { get; init; }

        private Url(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentException("URL can not be empty."); }
            if ( Uri.IsWellFormedUriString(value, UriKind.Absolute)) { throw new ArgumentException("URL is not formatted correctly."); }

            Value = value;
        }

        public static Result<Url> Create(string value) 
        {
            if (string.IsNullOrWhiteSpace(value)) { return Result<Url>.Failure("URL can not be empty."); }
            if (Uri.IsWellFormedUriString(value, UriKind.Absolute)) { return Result<Url>.Failure("URL is not formatted correctly."); }

            Url url = new Url(value);

            return Result<Url>.Success(url);
        }
    }
}
