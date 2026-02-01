using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateRegisterWithoutVoucherValidator
    {
        private readonly RegisterInsertWithoutVoucherValidator _validator;

        public CreateRegisterWithoutVoucherValidator() 
        {
            _validator = new RegisterInsertWithoutVoucherValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Client_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertRegisterWithoutVoucher(
                    clientId: Guid.Empty,
                    registerTypeId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Client);
        }

        [Fact]
        public void Should_Have_Error_When_Register_Type_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertRegisterWithoutVoucher(
                    clientId: Guid.NewGuid(),
                    registerTypeId: Guid.Empty
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RegisterType);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTInsertRegisterWithoutVoucher(
                    clientId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
