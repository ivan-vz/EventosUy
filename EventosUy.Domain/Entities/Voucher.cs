using EventosUy.Domain.Enumerates;

namespace EventosUy.Domain.Entities
{
    public class Voucher(string name, string code, int discount, int quota, bool automatic, Guid editionId, Guid registerTypeId, Guid? sponsorId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = name;
        public string Code { get; init; } = code;
        public int Discount { get; init; } = discount;
        public int Quota { get; init; } = quota;
        public int Used { get; set; } = 0;
        public bool IsAutoApplied { get; init; } = automatic;
        public DateTimeOffset Created { get; init; } = DateTimeOffset.Now;
        public VoucherState State { get; set; } = VoucherState.AVAILABLE;
        public Guid Edition { get; init; } = editionId;
        public Guid RegisterType { get; init; } = registerTypeId;
        public Guid? Sponsor { get; init; } = sponsorId;

        /*
        public static Result<Voucher> Create(string name, int discount, int quota, bool automatic, Edition edition, RegisterType registerType) 
        {
            List<string> errors = [];
     
            if (discount <= 0 || discount > 100) { errors.Add("Invalid discount."); }
            if (quota <= 0) { errors.Add("Quota must be greater than 0."); }
            if (edition.To < DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Invalid expiration date."); }
            if (registerType.Edition != edition.Id) { errors.Add($"Register type {registerType.Name} is not available for Edition {edition.Name}"); }
            
            if (errors.Count != 0) { return Result<Voucher>.Failure(errors); }

            string code = Guid.NewGuid().ToString("N")[0..8].ToUpper();
            Voucher instance = new(name, code, discount, quota, automatic, edition.To, edition.Id, registerType.Id);

            return Result<Voucher>.Success(instance);
        }
        
        public static Result<Voucher> Create(string name, string code, int discount, int quota, bool automatic, Edition edition, RegisterType registerType)
        {
            List<string> errors = [];

            if (discount <= 0) { errors.Add("Invalid discount."); }
            if (quota <= 0) { errors.Add("Quota must be greater than 0."); }
            if (edition.To < DateOnly.FromDateTime(DateTime.UtcNow)) { errors.Add("Invalid expiration date."); }
            if (registerType.Edition != edition.Id) { errors.Add($"Register type {registerType.Name} is not available for Edition {edition.Name}"); }

            if (errors.Count != 0) { return Result<Voucher>.Failure(errors); }

            Voucher instance = new(name, code, discount, quota, automatic, edition.To, edition.Id, registerType.Id);

            return Result<Voucher>.Success(instance);
        }*/
    }
}
