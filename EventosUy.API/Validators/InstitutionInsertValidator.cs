using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class InstitutionInsertValidator : AbstractValidator<DTInsertInstitution>
    {
        public InstitutionInsertValidator() 
        {
            RuleFor(x => x.Nickname).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Acronym).NotEmpty()
                .MinimumLength(2)
                .MaximumLength(10)
                .Must(value => value.All(c => char.IsLetter(c)));
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Url).NotEmpty()
                .Must(value => Uri.IsWellFormedUriString(value, UriKind.Absolute)).WithMessage("URL is not formatted correctly.");
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.Number).NotEmpty().Length(4);
            RuleFor(x => x.Floor).NotNull();
        }
    }
}