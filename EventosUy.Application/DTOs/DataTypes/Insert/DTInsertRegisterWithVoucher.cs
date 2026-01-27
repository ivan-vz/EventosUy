namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertRegisterWithVoucher(Guid clientId, Guid editionId, Guid registerTypeId, string code)
    {
        public Guid Client { get; init; } = clientId;
        public Guid Edition { get; init; } = editionId;
        public Guid RegisterType { get; init; } = registerTypeId;
        public string Code { get; init; } = code;
    }
}
