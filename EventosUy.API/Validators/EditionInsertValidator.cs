using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class EditionInsertValidator : AbstractValidator<DTInsertEdition>
    {
        public EditionInsertValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Initials).NotEmpty();
            RuleFor(x => x.From).NotEmpty().Must(date => date > DateOnly.FromDateTime(DateTime.UtcNow));
            RuleFor(x => x.To).NotEmpty().Must(date => date > DateOnly.FromDateTime(DateTime.UtcNow));
            RuleFor(x => x.To).GreaterThanOrEqualTo(x => x.From);
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.Floor).NotEmpty();
            RuleFor(x => x.Event).NotEmpty();
        }
    }
}
