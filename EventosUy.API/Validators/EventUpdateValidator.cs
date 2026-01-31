using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class EventUpdateValidator : AbstractValidator<DTUpdateEvent>
    {
        public EventUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Initials).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Categories).NotEmpty().WithMessage("Include at least 1 category.");
            RuleForEach(x => x.Categories).NotEmpty().WithMessage("Categories cannot be empties.");
        }
    }
}
