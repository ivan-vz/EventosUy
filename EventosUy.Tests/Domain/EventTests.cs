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
            Assert.Equal("Name cannot be empty.", result.Errors[0]);
            Assert.Equal("Initials cannot be empty.", result.Errors[1]);
            Assert.Equal("Description cannot be empty.", result.Errors[2]);
            Assert.Equal("Institution cannot be empty.", result.Errors[3]);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData() 
        {
            // Arrange
            var institutionInstance = Institution.Create(
                "nickname", 
                Password.Create("PassWord123").Value!, 
                Email.Create("institution@gmail.com").Value!, 
                "name", 
                Url.Create("https://inst.com").Value!, 
                Address.Create("country", "city", "street", "0124").Value!, 
                "description"
                ).Value!;

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
