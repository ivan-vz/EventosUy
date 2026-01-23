using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class EventInsertValidator : AbstractValidator<DTInsertEvent>
    {
        public EventInsertValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Initials).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Categories).NotEmpty();
            RuleFor(x => x.Institution).NotNull().Must(id => id != Guid.Empty);
        }
    }
}
