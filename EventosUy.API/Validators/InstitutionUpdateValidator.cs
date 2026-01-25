using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class InstitutionUpdateValidator : AbstractValidator<DTUpdateInstitution>
    {
        public InstitutionUpdateValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nickname).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Url).NotEmpty()
                .Must(value => Uri.IsWellFormedUriString(value, UriKind.Absolute)).WithMessage("URL is not formatted correctly.");
        }
    }
}
