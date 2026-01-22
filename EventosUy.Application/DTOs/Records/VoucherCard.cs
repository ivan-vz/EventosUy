using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.DTOs.Records
{
    public record VoucherCard(Guid Id, string Name, VoucherState State);
}
