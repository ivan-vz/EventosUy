using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateRegisterWithVoucherValidator
    {
        private readonly RegisterInsertWithVoucherValidator _validator;

        public CreateRegisterWithVoucherValidator() 
        {
            _validator = new RegisterInsertWithVoucherValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Client_Is_Invalid() 
        {
            // Arrange
            var model = new DTInsertRegisterWithVoucher(
                    clientId: Guid.Empty,
                    registerTypeId: Guid.NewGuid(),
                    code: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate( model );

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Client);
        }

        [Fact]
        public void Should_Have_Error_When_Register_Type_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertRegisterWithVoucher(
                    clientId: Guid.NewGuid(),
                    registerTypeId: Guid.Empty,
                    code: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RegisterType);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("codeCOD")]
        public void Should_Have_Error_When_Code_Is_Invalid(string code)
        {
            // Arrange
            var model = new DTInsertRegisterWithVoucher(
                    clientId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    code: code
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTInsertRegisterWithVoucher(
                    clientId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    code: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
