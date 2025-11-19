namespace EventosUy.Dominio.ValueObjects
{
    public record Email
    {
        public string Value { get; init; }

        public Email(string value) 
        {
            if (string.IsNullOrWhiteSpace(value) && !value.Contains('@')) { throw new ArgumentException("El email no tiene el formato correcto."); }

            Value = value;
        }
    }
}
