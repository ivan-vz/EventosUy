namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTRegisterType
    {
        public string Name { get; init; }
        public string Edition { get; init; }
        public string Description { get; init; }
        public float Price { get; init; }
        public int Quota { get; init; }
        public DateOnly Created { get; init; }

        public DTRegisterType(string name, string edition, string description, float price, int quota, DateOnly created) 
        {
            Name = name;
            Edition = edition;
            Description = description;
            Price = price;
            Quota = quota;
            Created = created;
        }
    }
}
