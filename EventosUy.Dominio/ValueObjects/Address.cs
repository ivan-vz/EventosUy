namespace EventosUy.Dominio.ValueObjects
{
    public record Address
    {
        public string Country { get; init; }
        public string City { get; init; }
        public string Street { get; init; }
        public string Number { get; init; }

        public Address(string country, string city, string street, string number) 
        {
            if (string.IsNullOrWhiteSpace(country)) { throw new ArgumentException("Address Country can not be empty."); }
            if (string.IsNullOrWhiteSpace(city)) { throw new ArgumentException("Address City can not be empty."); }
            if (string.IsNullOrWhiteSpace(street)) { throw new ArgumentException("Address Street can not be empty."); }
            if (string.IsNullOrWhiteSpace(number) && number.Length != 4) { throw new ArgumentException("Address Number can not be empty."); }

            Country = country;
            City = city;
            Street = street;
            Number = number;
        }
    }
}
