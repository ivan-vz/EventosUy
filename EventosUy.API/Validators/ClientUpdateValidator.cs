using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class ClientUpdateValidator : AbstractValidator<DTUpdateClient>
    {
        public ClientUpdateValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nickname).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(12)
                .MaximumLength(64)
                .Must(value => value.Any(c => char.IsLower(c))).WithMessage("Password must contain lower cases.")
                .Must(value => value.Any(c => char.IsUpper(c))).WithMessage("Password must contain upper cases.")
                .Must(value => value.Any(c => char.IsDigit(c))).WithMessage("Password must contain digits.");
        }
    }
}
