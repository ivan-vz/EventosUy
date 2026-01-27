using EventosUy.Application.DTOs.Records;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTVoucher(Guid id, string name, int discount, int quota, int used, DateTimeOffset created, VoucherState state, ActivityCard editionCard, RegisterTypeCard registerTypeCard)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; init; } = name;
        public int Discount { get; init; } = discount;
        public int Quota { get; init; } = quota;
        public int Used { get; init; } = used;
        public DateTimeOffset Created { get; init; } = created;
        public VoucherState State { get; init; } = state;
        public ActivityCard Edition { get; init; } = editionCard;
        public RegisterTypeCard RegisterType { get; init; } = registerTypeCard;
    }
}
