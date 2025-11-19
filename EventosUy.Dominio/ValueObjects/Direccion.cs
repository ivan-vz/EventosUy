namespace EventosUy.Dominio.ValueObjects
{
    public record Direccion
    {
        public string Pais { get; init; }
        public string Ciudad { get; init; }
        public string Calle { get; init; }
        public string Numero { get; init; }

        public Direccion(string pais, string ciudad, string calle, string numero) 
        {
            if (string.IsNullOrWhiteSpace(pais)) { throw new ArgumentException("El pais no puede ser vacio."); }
            if (string.IsNullOrWhiteSpace(ciudad)) { throw new ArgumentException("La ciudad no puede ser vacia."); }
            if (string.IsNullOrWhiteSpace(calle)) { throw new ArgumentException("La calle no puede ser vacia."); }
            if (string.IsNullOrWhiteSpace(calle) && calle.Length != 4) { throw new ArgumentException("La calle no puede ser vacia o no tener 4 digitos."); }

            Pais = pais;
            Ciudad = ciudad;
            Calle = calle;
            Numero = numero;
        }
    }
}
