using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateVoucherWithoutSponsorValidator
    {
        private readonly VoucherInsertWithoutSponsorValidator _validator;

        public CreateVoucherWithoutSponsorValidator() 
        {
            _validator = new VoucherInsertWithoutSponsorValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name) 
        {
            // Arrange
            var model = new DTInsertVoucherWithoutSponsor(
                    name: name,
                    code: "codeCODE",
                    discount: 100,
                    automatic: true,
                    registerTypeId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate( model );

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("ABCD123")]
        public void Should_Have_Error_When_Code_Is_Invalid(string code)
        {
            // Arrange
            var model = new DTInsertVoucherWithoutSponsor(
                    name: "name",
                    code: code,
                    discount: 100,
                    automatic: true,
                    registerTypeId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(101)]
        public void Should_Have_Error_When_Discount_Is_Invalid(int discount)
        {
            // Arrange
            var model = new DTInsertVoucherWithoutSponsor(
                    name: "name",
                    code: "codeCODE",
                    discount: discount,
                    automatic: true,
                    registerTypeId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Discount);
        }

        [Fact]
        public void Should_Have_Error_When_Register_Type_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertVoucherWithoutSponsor(
                    name: "name",
                    code: "codeCODE",
                    discount: 100,
                    automatic: true,
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
            var model = new DTInsertVoucherWithoutSponsor(
                    name: "name",
                    code: "codeCODE",
                    discount: 65,
                    automatic: true,
                    registerTypeId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
