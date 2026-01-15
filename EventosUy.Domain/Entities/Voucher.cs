using EventosUy.Domain.Common;
using EventosUy.Domain.DTOs.DataTypes;
using EventosUy.Domain.DTOs.Records;
using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Voucher
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
        public int Discount { get; init; }
        public int Quota { get; init; }
        public int Used { get; init; }
        public bool IsAutoApplied { get; init; }
        public DateTimeOffset Created { get; init; }
        public DateOnly Expired { get; init; }
        public VoucherState State { get; init; }
        public Guid Edition { get; init; }

        private Voucher(string name, string code, int discount, int quota, bool automatic, DateOnly expired, Guid editionId) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Code = code;
            Discount = discount;
            Quota = quota;
            Used = 0;
            IsAutoApplied = automatic;
            Created = DateTimeOffset.Now;
            Expired = expired;
            State = VoucherState.AVAILABLE;
            Edition = editionId;
        }

        public static Result<Voucher> Create(string name, int discount, int quota, bool automatic, DateOnly expired, Guid editionId) 
        {
            List<string> errors = [];
            if (string.IsNullOrEmpty(name)) { errors.Add("Name cannot be empty."); }
            if (discount <= 0 || discount > 100) { errors.Add("Invalid discount."); }
            if (quota <= 0) { errors.Add("Quota must be greater than 0."); }
            if (expired < DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Invalid expiration date."); }
            if (editionId == Guid.Empty) { errors.Add("Edition cannot be empty."); }

            if (errors.Count != 0) { return Result<Voucher>.Failure(errors); }

            string code = Guid.NewGuid().ToString("N")[0..8].ToUpper();
            Voucher instance = new(name, code, discount, quota, automatic, expired, editionId);

            return Result<Voucher>.Success(instance);
        }

        public static Result<Voucher> Create(string name, string code, int discount, int quota, bool automatic, DateOnly expired, Guid editionId)
        {
            List<string> errors = [];
            if (string.IsNullOrEmpty(name)) { errors.Add("Name cannot be empty."); }
            if (string.IsNullOrWhiteSpace(code)) { errors.Add("Code cannot be empty."); }
            if (discount <= 0) { errors.Add("Invalid discount."); }
            if (quota <= 0) { errors.Add("Quota must be greater than 0."); }
            if (editionId == Guid.Empty) { errors.Add("Edition cannot be empty."); }
            if (expired < DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Invalid expiration date."); }
            if (errors.Count != 0) { return Result<Voucher>.Failure(errors); }

            Voucher instance = new(name, code, discount, quota, automatic, expired, editionId);

            return Result<Voucher>.Success(instance);
        }
        
        public DTVoucher GetDT(Edition edition) { return new(Name, Discount, Quota, Used, Created, Expired, State, edition); }

        public VoucherCard GetCard() { return new(Id, Name, State); }
    }
}
