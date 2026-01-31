using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class UpdateEditionValidator
    {
        private readonly EditionUpdateValidator _validator;

        public UpdateEditionValidator() 
        {
            _validator = new EditionUpdateValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Invalid()
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.Empty,
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: "city",
                    street: "street",
                    number: "0123",
                    floor: 0
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
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: name,
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: "city",
                    street: "street",
                    number: "0123",
                    floor: 0
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
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: initials,
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: "city",
                    street: "street",
                    number: "0123",
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Initials);
        }

        [Theory]
        [InlineData("2024-08-10", "2024-07-10", "From", "From must be a future date.")]
        [InlineData("2024-08-10", "2024-07-10", "To", "To must be a future date.")]
        [InlineData("2030-08-20", "2030-08-10", "To", "To must be greater than or equal to From.")]
        public void Should_Have_Error_When_Date_Is_Invalid(string from, string to, string property, string expected)
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse(from),
                    to: DateOnly.Parse(to),
                    country: "country",
                    city: "city",
                    street: "street",
                    number: "0123",
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(property).WithErrorMessage(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Country_Is_Invalid(string country)
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: country,
                    city: "city",
                    street: "street",
                    number: "0123",
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Country);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_City_Is_Invalid(string city)
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: city,
                    street: "street",
                    number: "0123",
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Street_Is_Invalid(string street)
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: "city",
                    street: street,
                    number: "0123",
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Street);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abc12")]
        [InlineData("123")]
        public void Should_Have_Error_When_Number_Is_Invalid(string number)
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: "city",
                    street: "street",
                    number: number,
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Number);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTUpdateEdition(
                    id: Guid.NewGuid(),
                    name: "name",
                    initials: "initials",
                    from: DateOnly.Parse("2026-08-10"),
                    to: DateOnly.Parse("2026-09-10"),
                    country: "country",
                    city: "city",
                    street: "street",
                    number: "0123",
                    floor: 0
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
