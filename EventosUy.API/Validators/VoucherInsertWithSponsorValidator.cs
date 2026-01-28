using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class VoucherInsertWithSponsorValidator : AbstractValidator<DTInsertVoucherWithSponsor>
    {
        public VoucherInsertWithSponsorValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Discount).NotNull().ExclusiveBetween(0, 100);
            RuleFor(x => x.Automatic).NotNull();
            RuleFor(x => x.Sponsor).NotEmpty();
        }
    }
}
