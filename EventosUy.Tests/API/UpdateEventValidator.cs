using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation.TestHelper;
using System.Xml.Linq;

namespace EventosUy.Tests.API
{
    public class UpdateEventValidator
    {
        private readonly EventUpdateValidator _validator;

        public UpdateEventValidator() 
        {
            _validator = new EventUpdateValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Invalid()
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.Empty,
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: ["cat1", "cat2"]
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name)
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.NewGuid(),
                    name: name,
                    initials: "ABC",
                    description: "description",
                    categories: ["cat1", "cat2"]
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Initials_Is_Invalid(string initials)
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: initials,
                    description: "description",
                    categories: ["cat1", "cat2"]
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Initials);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Description_Is_Invalid(string description)
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "ABC",
                    description: description,
                    categories: ["cat1", "cat2"]
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Categories_Is_Empty()
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: []
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Categories).WithErrorMessage("Include at least 1 category.");
        }

        [Fact]
        public void Should_Have_Error_When_Categories_Is_Invalid()
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: ["", " "]
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Categories).WithErrorMessage("Categories cannot be empties.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTUpdateEvent
                (
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: ["cat1", "cat2"]
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
