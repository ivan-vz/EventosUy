using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class RegisterTypeInsertValidator : AbstractValidator<DTInsertRegisterType>
    {
        public RegisterTypeInsertValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Quota).NotNull().GreaterThan(0);
            RuleFor(x => x.Edition).NotEmpty();
        }
    }
}
