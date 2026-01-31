using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public  class UpdateClientValidator
    {
        private readonly ClientUpdateValidator _validator;

        public UpdateClientValidator()
        {
            _validator = new ClientUpdateValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Invalid()
        {
            // Arrange
            var model = new DTUpdateClient(
                id: Guid.Empty,
                nickname: "nickname",
                password: "Password0125",
                email: "email@gmail.com"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Nickname_Is_Invalid(string nickname)
        {
            // Arrange
            var model = new DTUpdateClient(
                id: Guid.NewGuid(),
                nickname: nickname,
                password: "Password0125",
                email: "email@gmail.com"
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
            var model = new DTUpdateClient(
                id: Guid.NewGuid(),
                nickname: "nickname",
                password: password,
                email: "email@gmail.com"
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
            var model = new DTUpdateClient(
                id: Guid.NewGuid(),
                nickname: "nickname",
                password: password,
                email: "email@gmail.com"
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
            var model = new DTUpdateClient(
                id: Guid.NewGuid(),
                nickname: "nickname",
                password: "Password0125",
                email: email
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTUpdateClient(
                id: Guid.NewGuid(),
                nickname: "nickname",
                password: "Password0125",
                email: "email@gmail.com"
            );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
