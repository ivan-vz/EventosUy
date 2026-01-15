using EventosUy.Domain.Common;

namespace EventosUy.Domain.ValueObjects
{
    public record Address
    {
        public string Country { get; init; }
        public string City { get; init; }
        public string Street { get; init; }
        public string Number { get; init; }
        public int? Floor { get; init; }
        public string FullAddress { get; init; }

        private Address(string country, string city, string street, string number, int? floor) 
        {
            Country = country;
            City = city;
            Street = street;
            Number = number;
            Floor = floor;
            FullAddress = $"{country}, {city}, {Street}, {Number}, {floor}";
        }

        public static Result<Address> Create(string country, string city, string street, string number, int? floor) 
        {
            List<string> errors = [];
            if (string.IsNullOrWhiteSpace(country)) { errors.Add("Address Country can not be empty."); }
            if (string.IsNullOrWhiteSpace(city)) { errors.Add("Address City can not be empty."); }
            if (string.IsNullOrWhiteSpace(street)) { errors.Add("Address Street can not be empty."); }
            if (string.IsNullOrWhiteSpace(number) && number.Length != 4) { errors.Add("Address Number can not be empty."); }

            if (errors.Count != 0) { return Result<Address>.Failure(errors); }

            Address address = new(country, city, street, number, floor);

            return Result<Address>.Success(address);
        }
    }
}
