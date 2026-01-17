using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTRegister(DateTimeOffset created, decimal total, RegisterTypeCard registerTypeCard, ActivityCard editionCard, UserCard clientCard, VoucherCard? voucherCard)
    {
        public decimal Total { get; init; } = total;
        public DateTimeOffset Created { get; init; } = created;
        public RegisterTypeCard RegisterType { get; init; } = registerTypeCard;
        public ActivityCard Edition { get; init; } = editionCard;
        public UserCard Client { get; init; } = clientCard;
        public VoucherCard? Voucher { get; init; } = voucherCard;
    }
}
