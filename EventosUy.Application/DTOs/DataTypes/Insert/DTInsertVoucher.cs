namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertVoucher(string name, string code, int discount, int quota, bool automatic, Guid editionId, Guid registerTypeId, Guid? sponsorshipId)
    {
        public string Name { get; init; } = name;
        public string Code { get; init; } = code;
        public int Discount { get; init; } = discount;
        public int quota { get; init; } = quota;
        public bool Automatic { get; init; } = automatic;
        public Guid EditionId { get; init;} = editionId;
        public Guid RegisterTypeId { get; init;} = registerTypeId;
        public Guid? SponsorId { get; init; } = sponsorshipId;
    }
}
