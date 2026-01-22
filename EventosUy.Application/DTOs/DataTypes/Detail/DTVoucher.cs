using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Application.DTOs.DataTypes.Detail
{
    public class DTVoucher(string name, int discount, int quota, int used, DateTimeOffset created, DateOnly expired, VoucherState state, Edition edition)
    {
        public string Name { get; init; } = name;
        public int Discount { get; init; } = discount;
        public int Quota { get; init; } = quota;
        public int Used { get; init; } = used;
        public DateTimeOffset Created { get; init; } = created;
        public DateOnly Expired { get; init; } = expired;
        public VoucherState State { get; init; } = state;
        public ActivityCard Edition { get; init; } = edition.GetCard();
    }
}
