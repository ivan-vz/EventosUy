namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertVoucherWithoutSponsor(string name, string code, int discount, bool automatic, Guid registerTypeId)
    {
        public string Name { get; init; } = name;
        public string Code { get; init; } = code;
        public int Discount { get; init; } = discount;
        public bool Automatic { get; init; } = automatic;
        public Guid RegisterType { get; init; } = registerTypeId;
    }
}
