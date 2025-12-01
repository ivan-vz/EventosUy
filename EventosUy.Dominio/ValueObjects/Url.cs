namespace EventosUy.Dominio.ValueObjects
{
    public record Url
    {
        public string Value { get; init; }

        public Url(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentException("URL can not be empty."); }
            if ( Uri.IsWellFormedUriString(value, UriKind.Absolute)) { throw new ArgumentException("URL is not formatted correctly."); }

            Value = value;
        }
    }
}
