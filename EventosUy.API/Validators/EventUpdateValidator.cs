using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class EventUpdateValidator : AbstractValidator<DTUpdateEvent>
    {
        public EventUpdateValidator()
        {
            RuleFor(x => x.Id).NotNull().Must(id => id != Guid.Empty);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Initials).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Categories).NotEmpty();
        }
    }
}
