using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class InstitutionTests
    {

        [Fact]
        public void Create_WithValidInput_ReturnsSuccess()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;

            // Act
            var result = Institution.Create("nickname", password, email, "name", url, address, "description");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("nickname", result.Value.Nickname);
            Assert.Equal("name", result.Value.Name);
            Assert.Equal("description", result.Value.Description);
            Assert.True(result.Value.Password.Verify("PassWord1234"));
            Assert.Equal(email, result.Value.Email);
            Assert.Equal(url, result.Value.Url);
            Assert.Equal(address, result.Value.Address);
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData(" ", " ", " ")]
        public void Create_WithInvalidInput_ReturnsFailure(string nickname, string name, string description)
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;

            // Act
            var result = Institution.Create(nickname, password, email, name, url, address, description);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(3, result.Errors.Count);
            Assert.Equal("Nickname cannot be empty.", result.Errors[0]);
            Assert.Equal("Name cannot be empty.", result.Errors[1]);
            Assert.Equal("Description cannot be empty.", result.Errors[2]);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, address, "description").Value!;

            // Act
            var dtInstitution = institutionInstance.GetDT();

            // Assert
            Assert.NotNull(dtInstitution);
            Assert.Equal(institutionInstance.Nickname, dtInstitution.Nickname);
            Assert.Equal(institutionInstance.Name, dtInstitution.Name);
            Assert.Equal(institutionInstance.Description, dtInstitution.Description);
            Assert.Equal(institutionInstance.Email.Value, dtInstitution.Email);
            Assert.Equal(institutionInstance.Url.Value, dtInstitution.Url);
            Assert.Equal(institutionInstance.Address.FullAddress, dtInstitution.Address);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, address, "description").Value!;

            // Act
            var card = institutionInstance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(institutionInstance.Id, card.Id);
            Assert.Equal(institutionInstance.Nickname, card.Nickname);
            Assert.Equal(institutionInstance.Email.Value, card.Email);
        }
    }
}
