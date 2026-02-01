using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateRegisterTypeValidator
    {
        private readonly RegisterTypeInsertValidator _validator;

        public CreateRegisterTypeValidator() 
        {
            _validator = new RegisterTypeInsertValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name) 
        {
            // Arrange
            var model = new DTInsertRegisterType(
                    name: name,
                    description: "description",
                    price: 0,
                    quota: 1,
                    editionId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate( model );

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Description_Is_Invalid(string description)
        {
            // Arrange
            var model = new DTInsertRegisterType(
                    name: "name",
                    description: description,
                    price: 0,
                    quota: 1,
                    editionId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertRegisterType(
                    name: "name",
                    description: "description",
                    price: -1,
                    quota: 1,
                    editionId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Quota_Is_Invalid(int quota)
        {
            // Arrange
            var model = new DTInsertRegisterType(
                    name: "name",
                    description: "description",
                    price: 0,
                    quota: quota,
                    editionId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quota);
        }

        [Fact]
        public void Should_Have_Error_When_Edition_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertRegisterType(
                    name: "name",
                    description: "description",
                    price: 0,
                    quota: 1,
                    editionId: Guid.Empty
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Edition);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTInsertRegisterType(
                    name: "name",
                    description: "description",
                    price: 0,
                    quota: 1,
                    editionId: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
