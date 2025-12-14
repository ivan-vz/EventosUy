using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class EventTests
    {

        [Fact]
        public void Create_WithValidInput_ReturnsSuccess() 
        {
            // Act
            Guid institutionId = Guid.NewGuid();
            var result = Event.Create("NewEvent", "NE", "Description", institutionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("NewEvent", result.Value.Name);
            Assert.Equal("NE", result.Value.Initials);
            Assert.Equal("Description", result.Value.Description);
            Assert.Equal(institutionId, result.Value.Institution);
        }

        public static IEnumerable<object[]> InvalidInputs() 
        {
            yield return new object[] { "", "", "", Guid.Empty };
            yield return new object[] { " ", " ", " ", Guid.Empty };
        }

        [Theory]
        [MemberData(nameof(InvalidInputs))]
        public void Create_WithInvalidInput_ReturnsFailure(string name, string initials, string description, Guid institution) 
        {
            // Act
            var result = Event.Create(name, initials, description, institution);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(4, result.Errors.Count);
            Assert.Contains("Name cannot be empty.", result.Errors);
            Assert.Contains("Initials cannot be empty.", result.Errors);
            Assert.Contains("Description cannot be empty.", result.Errors);
            Assert.Contains("Institution cannot be empty.", result.Errors);
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

            var eventInstance = Event.Create("event", "ev", "description", institutionInstance.Id).Value!;

            // Act
            var dtEvent = eventInstance.GetDT(institutionInstance);

            // Assert
            Assert.NotNull(dtEvent);
            Assert.Equal(institutionInstance.Name, dtEvent.Institution);
            Assert.Equal(eventInstance.Name, dtEvent.Name);
            Assert.Equal(eventInstance.Initials, dtEvent.Initials);
            Assert.Equal(eventInstance.Description, dtEvent.Description);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData() 
        {
            // Arrange
            var eventInstance = Event.Create("event", "ev", "description", Guid.NewGuid()).Value!;

            // Act
            var card = eventInstance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(eventInstance.Id, card.Id);
            Assert.Equal(eventInstance.Name, card.Name);
            Assert.Equal(eventInstance.Initials, card.Initials);
        }
    }
}
