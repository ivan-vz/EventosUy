using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateInstitutionValidator
    {
        private readonly InstitutionInsertValidator _validator;
        public CreateInstitutionValidator() 
        {
            _validator = new InstitutionInsertValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Nickname_Is_Invalid(string nickname)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: nickname,
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Nickname);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Password")]
        [InlineData("Password0123456789789456123012365478987415141541154171145144154154")]
        public void Should_Have_Error_When_Password_Is_Invalid_Length(string password)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: password,
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("PASSWORD1234", "Password must contain lower cases.")]
        [InlineData("password1234", "Password must contain upper cases.")]
        [InlineData("PasswordPASS", "Password must contain digits.")]
        public void Should_Have_Error_When_Password_Is_Invalid_Format(string password, string expected)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: password,
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("email.com")]
        [InlineData("@")]
        [InlineData("@gmail.com")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "",
                password: "Password0123",
                email: email,
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: name,
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("a2cdefghijkl")]
        [InlineData("123456789")]
        public void Should_Have_Error_When_Acronym_Is_Invalid(string acronym)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: acronym,
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Acronym);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Description_Is_Invalid(string description)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: description,
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("http://")]
        [InlineData("http://invalid .com")]
        [InlineData("invalid.com")]
        public void Should_Have_Error_When_Url_Is_Invalid(string url)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: url,
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Url).WithErrorMessage("URL is not formatted correctly.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Country_Is_Invalid(string country)
        {
            // Arrange
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: country,
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
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
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: city,
                street: "street",
                number: "0214",
                floor: 2
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
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: street,
                number: "0214",
                floor: 2
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
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: number,
                floor: 2
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
            var model = new DTInsertInstitution(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                name: "name",
                acronym: "acronym",
                description: "description",
                url: "https://www.url.com",
                country: "country",
                city: "city",
                street: "street",
                number: "0214",
                floor: 2
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
