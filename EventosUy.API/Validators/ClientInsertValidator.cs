using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class ClientInsertValidator : AbstractValidator<DTInsertClient>
    {
        public ClientInsertValidator() 
        {
            RuleFor(x => x.Nickname).NotEmpty();
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