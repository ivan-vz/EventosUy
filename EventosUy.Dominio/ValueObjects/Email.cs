namespace EventosUy.Dominio.ValueObjects
{
    public record Email
    {
        public string Value { get; init; }

        public Email(string value) 
        {
            if (string.IsNullOrWhiteSpace(value) && !value.Contains('@')) { throw new ArgumentException("Email is not formatted correctly."); }

            Value = value;
        }
    }
}
