using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class VoucherInsertWithoutSponsorValidator : AbstractValidator<DTInsertVoucherWithoutSponsor>
    {
        public VoucherInsertWithoutSponsorValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Discount).NotNull().InclusiveBetween(1, 100);
            RuleFor(x => x.RegisterType).NotEmpty();
        }
    }
}
