using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class EditionTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess() 
        {
            //Arrange string name, string initials, DateOnly from, DateOnly to, Address address, Guid eventId, Guid institutionId
            var address = Address.Create("country", "city", "street", "number").Value!;
            var eventId = Guid.NewGuid();
            var institutionId = Guid.NewGuid();
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);

            // Act
            var result = Edition.Create("name", "initials", from, to, address, eventId, institutionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("name", result.Value.Name);
            Assert.Equal("initials", result.Value.Initials);
            Assert.Equal(from, result.Value.From);
            Assert.Equal(to, result.Value.To);
            Assert.Equal(address, result.Value.Address);
            Assert.Equal(eventId, result.Value.Event);
            Assert.Equal(institutionId, result.Value.Institution);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void Create_WithInvalidInput_ReturnsFailure(string name, string initials)
        {
            //Arrange string name, string initials, DateOnly from, DateOnly to, Address address, Guid eventId, Guid institutionId
            var address = Address.Create("country", "city", "street", "number").Value!;
            var eventId = Guid.NewGuid();
            var institutionId = Guid.NewGuid();
            var from = new DateOnly(2000, 8, 8);
            var to = new DateOnly(1990, 12, 8);

            // Act
            var result = Edition.Create(name, initials, from, to, address, eventId, institutionId);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(4, result.Errors.Count);
            Assert.Contains("Name cannot be empty.", result.Errors);
            Assert.Contains("Initials cannot be empty.", result.Errors);
            Assert.Contains("The start of editing cannot be later than its completion.", result.Errors);
            Assert.Contains("The start date of the edition cannot be earlier than today's date.", result.Errors);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var institutionAddress = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, institutionAddress, "description").Value!;
            var eventInstance = Event.Create("event", "ev", "description", institutionInstance.Id).Value!;

            var editionAddress = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, editionAddress, eventInstance.Id, institutionInstance.Id).Value!;

            // Act
            var dtEdition = editionInstance.GetDT(eventInstance, institutionInstance);

            // Assert
            Assert.NotNull(dtEdition);
            Assert.Equal("name", dtEdition.Name);
            Assert.Equal("initials", dtEdition.Initials);
            Assert.Equal(from, dtEdition.From);
            Assert.Equal(to, dtEdition.To);
            Assert.Equal(editionAddress, dtEdition.Address);
            Assert.Equal(eventInstance.Name, dtEdition.Event);
            Assert.Equal(institutionInstance.Nickname, dtEdition.Institution);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData()
        {
            // Arrange
            var institutionId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var address = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, address, eventId, institutionId).Value!;

            // Act
            var card = editionInstance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(editionInstance.Id, card.Id);
            Assert.Equal("name", card.Name);
            Assert.Equal("initials", card.Initials);
        }
    }
}
