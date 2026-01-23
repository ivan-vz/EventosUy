namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTInstitution(
        Guid id, 
        string nickname, 
        string email, 
        string name, 
        string acronym, 
        string description, 
        string url,
        string country,
        string city,
        string street,
        string number,
        int floor, 
        DateTimeOffset created)
    {
        public Guid Id { get; init; } = id;
        public string Nickname { get; init; } = nickname;
        public string Email { get; init; } = email;
        public string Name { get; init; } = name;
        public string Acronym { get; init; } = acronym;
        public string Description { get; init; } = description;
        public string Url { get; init; } = url;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public string Number { get; init; } = number;
        public int Floor { get; init; } = floor;
        public DateTimeOffset Created { get; init; } = created;
    }
}
