using EventosUy.Application.DTOs.Records;
using EventosUy.Domain.DTOs.Records;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTRegister(Guid id, decimal total, DateTimeOffset created, RegisterTypeCard registerTypeCard, EditionCard editionCard, UserCard clientCard, VoucherCard? voucherCard)
    {
        public Guid Id { get; init; } = id;
        public decimal Total { get; init; } = total;
        public DateTimeOffset Created { get; init; } = created;
        public RegisterTypeCard RegisterType { get; init; } = registerTypeCard;
        public EditionCard Edition { get; init; } = editionCard;
        public UserCard Client { get; init; } = clientCard;
        public VoucherCard? Voucher { get; init; } = voucherCard;
    }
}
