using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation;

namespace EventosUy.API.Validators
{
    public class RegisterInsertWithoutVoucherValidator : AbstractValidator<DTInsertRegisterWithoutVoucher>
    {
        public RegisterInsertWithoutVoucherValidator() 
        {
            RuleFor(x => x.Client).NotEmpty();
            RuleFor(x => x.RegisterType).NotEmpty();
        }
    }
}
