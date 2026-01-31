using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class ClientInsertValidator : AbstractValidator<DTInsertClient>
    {
        public ClientInsertValidator() 
        {
            RuleFor(x => x.Nickname).NotEmpty();
            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(12)
                .MaximumLength(64)
                .Must(value => value.Any(c => char.IsLower(c))).WithMessage("Password must contain lower cases.")
                .Must(value => value.Any(c => char.IsUpper(c))).WithMessage("Password must contain upper cases.")
                .Must(value => value.Any(c => char.IsDigit(c))).WithMessage("Password must contain digits.");
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Birthday).Must(date => date < DateOnly.FromDateTime(DateTime.UtcNow));
            RuleFor(x => x.Ci).NotEmpty().Length(7);
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.FirstSurname).NotEmpty();
            RuleFor(x => x.LastSurname).NotEmpty();
        }
    }
}