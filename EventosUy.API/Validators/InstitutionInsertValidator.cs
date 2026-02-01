using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class InstitutionInsertValidator : AbstractValidator<DTInsertInstitution>
    {
        public InstitutionInsertValidator() 
        {
            RuleFor(x => x.Nickname).NotEmpty();
            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(12)
                .MaximumLength(64)
                .Must(value => value.Any(c => char.IsLower(c))).WithMessage("Password must contain lower cases.")
                .Must(value => value.Any(c => char.IsUpper(c))).WithMessage("Password must contain upper cases.")
                .Must(value => value.Any(c => char.IsDigit(c))).WithMessage("Password must contain digits.");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Acronym).NotEmpty()
                .MinimumLength(2)
                .MaximumLength(10)
                .Must(value => value.Any(c => char.IsLetter(c)));
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Url).NotEmpty()
                .Must(value => Uri.IsWellFormedUriString(value, UriKind.Absolute)).WithMessage("URL is not formatted correctly.");
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Number).NotEmpty().Length(4);
        }
    }
}