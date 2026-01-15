using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.DataTypes
{
    public class DTVoucher
    {
        public string Name { get; init; }
        public int Discount { get; init; }
        public int Quota { get; init; }
        public int Used { get; init; }
        public DateTimeOffset Created { get; init; }
        public DateOnly Expired { get; init; }
        public VoucherState State { get; init; }
        public ActivityCard Edition { get; init; }

        public DTVoucher(string name, int discount, int quota, int used, DateTimeOffset created, DateOnly expired, VoucherState state, Edition edition) 
        {
            Name = name;
            Discount = discount;
            Quota = quota;
            Used = used;
            Created = created;
            Expired = expired;
            State = state;
            Edition = edition.GetCard();
        }
    }
}
