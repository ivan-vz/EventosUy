using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class SponsorshipInsertValidator : AbstractValidator<DTInsertSponsorship>
    {
        public SponsorshipInsertValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Amount).NotNull().GreaterThan(0);
            RuleFor(x => x.Institution).NotEmpty();
            RuleFor(x => x.RegisterType).NotEmpty();
            RuleFor(x => x.VoucherName).NotEmpty();
            RuleFor(x => x.VoucherCode).NotEmpty().MinimumLength(8);
        }
    }
}
