using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class EditionUpdateValidator : AbstractValidator<DTUpdateEdition>
    {
        public EditionUpdateValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Initials).NotEmpty();
            RuleFor(x => x.From).NotEmpty().Must(date => date > DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("From must be a future date.");
            RuleFor(x => x.To).NotEmpty().Must(date => date > DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("To must be a future date.");
            RuleFor(x => x.To).GreaterThanOrEqualTo(x => x.From).WithMessage("To must be greater than or equal to From.");
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Number).NotEmpty().Length(4);
            RuleFor(x => x.Floor).NotNull();
        }
    }
}
