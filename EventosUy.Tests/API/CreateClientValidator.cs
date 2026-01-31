using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateClientValidator
    {
        private readonly ClientInsertValidator _validator;

        public CreateClientValidator() 
        {
            _validator = new ClientInsertValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Nickname_Is_Invalid(string nickname) 
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: nickname,
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate( model );

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
            var model = new DTInsertClient(
                nickname: "nickname",
                password: password,
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
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
            var model = new DTInsertClient(
                nickname: "nickname",
                password: password,
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
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
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: email,
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Birthday_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                ci: "0123456",
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Birthday);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("012345")]
        [InlineData("01234567")]
        public void Should_Have_Error_When_Ci_Is_Invalid(string ci)
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: ci,
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Ci);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_FirstName_Is_Invalid(string name)
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: name,
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_FirstSurname_Is_Invalid(string surname)
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "name",
                lastName: null,
                firstSurname: surname,
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstSurname);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_LastSurname_Is_Invalid(string surname)
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: surname
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastSurname);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTInsertClient(
                nickname: "nickname",
                password: "Password0123",
                email: "email@gmail.com",
                birthday: DateOnly.Parse("1999-07-13"),
                ci: "0123456",
                firstName: "Name",
                lastName: null,
                firstSurname: "FSurname",
                lastSurname: "LSurname"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
