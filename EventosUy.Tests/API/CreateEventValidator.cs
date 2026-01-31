using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateEventValidator
    {
        private readonly EventInsertValidator _validator;
        
        public CreateEventValidator() 
        {
            _validator = new EventInsertValidator();
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name) 
        {
            // Arrange
            var model = new DTInsertEvent
                (
                    name: name,
                    initials: "ABC",
                    description: "description",
                    categories: ["cat1", "cat2"],
                    institution: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate( model );

            // Assert
            result.ShouldHaveValidationErrorFor( x => x.Name );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Initials_Is_Invalid(string initials)
        {
            // Arrange
            var model = new DTInsertEvent
                (
                    name: "name",
                    initials: initials,
                    description: "description",
                    categories: ["cat1", "cat2"],
                    institution: Guid.NewGuid()
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
            var model = new DTInsertEvent
                (
                    name: "name",
                    initials: "ABC",
                    description: description,
                    categories: ["cat1", "cat2"],
                    institution: Guid.NewGuid()
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
            var model = new DTInsertEvent
                (
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: [],
                    institution: Guid.NewGuid()
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
            var model = new DTInsertEvent
                (
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: ["", " "],
                    institution: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Categories).WithErrorMessage("Categories cannot be empties.");
        }

        [Fact]
        public void Should_Have_Error_When_Institution_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertEvent
                (
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: ["cat1", "cat2"],
                    institution: Guid.Empty
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Institution);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTInsertEvent
                (
                    name: "name",
                    initials: "ABC",
                    description: "description",
                    categories: ["cat1", "cat2"],
                    institution: Guid.NewGuid()
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
