namespace EventosUy.Dominio.ValueObjects
{
    public record Url
    {
        public string Value { get; init; }

        public Url(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentException("La URL no puede ser vacia."); }
            if ( Uri.IsWellFormedUriString(value, UriKind.Absolute)) { throw new ArgumentException("La URL no tiene el formato correcta."); }

            Value = value;
        }
    }
}
