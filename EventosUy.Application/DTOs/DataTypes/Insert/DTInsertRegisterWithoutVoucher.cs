namespace EventosUy.Application.DTOs.DataTypes.Insert
{
    public class DTInsertRegisterWithoutVoucher(Guid clientId, Guid registerTypeId)
    {
        public Guid Client { get; init; } = clientId;
        public Guid RegisterType { get; init; } = registerTypeId;
    }
}
