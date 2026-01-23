namespace EventosUy.Domain.Entities
{
    public class Institution(
        string nickname, 
        string password, 
        string acronym, 
        string email,
        string name, 
        string description,
        string url, 
        string country,
        string city,
        string street,
        string number,
        int floor
        ) : User(nickname, password, email)
    {
        public string Name { get; init; } = name;
        public string Acronym { get; init; } = acronym;
        public string Description { get; set; } = description;
        public string Url { get; set; } = url;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public string Number { get; init; } = number;
        public int Floor { get; init; } = floor;
    }
}
