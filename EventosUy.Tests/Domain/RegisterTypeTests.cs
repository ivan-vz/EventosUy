using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class RegisterTypeTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess() 
        {
            // Arrange
            var editionId = Guid.NewGuid();

            // Act
            var result = RegisterType.Create("name", "description", 0, 1, editionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("name", result.Value.Name);
            Assert.Equal("description", result.Value.Description);
            Assert.Equal(0, result.Value.Price);
            Assert.Equal(1, result.Value.Quota);
            Assert.Equal(editionId, result.Value.Edition);
        }

        [Theory]
        [InlineData("", "", -1, -1)]
        [InlineData(" ", " ", -1, 0)]
        public void Create_WithInvalidInput_ReturnsFailure(string name, string description, decimal price, int quota) 
        {
            // Arrange
            var editionId = Guid.NewGuid();

            // Act
            var result = RegisterType.Create(name, description, price, quota, editionId);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(4, result.Errors.Count);
            Assert.Contains("Name cannot be empty.", result.Errors);
            Assert.Contains("Description cannot be empty.", result.Errors);
            Assert.Contains("Price must be greater than or equal to 0.", result.Errors);
            Assert.Contains("Quota must be greater than 0.", result.Errors);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var institutionId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var address = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, address, eventId, institutionId).Value!;

            var registerTypeIntsance = RegisterType.Create("name", "description", 0, 1, editionInstance.Id).Value!;
            
            // Act
            var dtRegisterType = registerTypeIntsance.GetDT(editionInstance);

            // Assert
            Assert.NotNull(dtRegisterType);
            Assert.Equal(registerTypeIntsance.Name, dtRegisterType.Name);
            Assert.Equal(registerTypeIntsance.Description, dtRegisterType.Description);
            Assert.Equal(registerTypeIntsance.Price, dtRegisterType.Price);
            Assert.Equal(registerTypeIntsance.Quota, dtRegisterType.Quota);
            Assert.Equal(editionInstance.Name, dtRegisterType.Edition);
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

            var registerTypeIntsance = RegisterType.Create("name", "description", 0, 1, editionInstance.Id).Value!;

            // Act
            var card = registerTypeIntsance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(registerTypeIntsance.Id, card.Id);
            Assert.Equal(registerTypeIntsance.Name, card.Name);
            Assert.Equal(registerTypeIntsance.Active, card.Active);
        }
    }
}
