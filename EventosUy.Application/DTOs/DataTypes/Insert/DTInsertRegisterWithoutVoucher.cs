namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertRegisterWithoutVoucher(Guid clientId, Guid editionId, Guid registerTypeId)
    {
        public Guid Client { get; init; } = clientId;
        public Guid Edition { get; init; } = editionId;
        public Guid RegisterType { get; init; } = registerTypeId;
    }
}
