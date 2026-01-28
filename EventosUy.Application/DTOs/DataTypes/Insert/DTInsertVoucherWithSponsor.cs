namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertVoucherWithSponsor(string name, string code, int discount, bool automatic, Guid sponsorshipId)
    {
        public string Name { get; init; } = name;
        public string Code { get; init; } = code;
        public int Discount { get; init; } = discount;
        public bool Automatic { get; init; } = automatic;
        public Guid Sponsor { get; init; } = sponsorshipId;
    }
}
