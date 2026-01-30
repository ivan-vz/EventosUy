using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class RegisterInsertWithVoucherValidator : AbstractValidator<DTInsertRegisterWithVoucher>
    {
        public RegisterInsertWithVoucherValidator() 
        {
            RuleFor(x => x.Client).NotEmpty();
            RuleFor(x => x.RegisterType).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
