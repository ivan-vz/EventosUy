using EventosUy.Domain.Entidades;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class RegisterTests
    {
        [Theory]
        [InlineData(0, Participation.EMPLOYEE)]
        [InlineData(0, Participation.GUEST)]
        [InlineData(100, Participation.CLIENT)]
        public void Create_WithValidInput_ReturnsSuccess(decimal total, Participation participation) 
        {
            // Arrange
            var personId = Guid.NewGuid();
            var editionId = Guid.NewGuid();
            var registerTypeId = Guid.NewGuid();

            // Act
            var result = Register.Create(total, "code", personId, editionId, registerTypeId, participation);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(total, result.Value.Total);
            Assert.Equal("code", result.Value.SponsorCode);
            Assert.Equal(participation, result.Value.Participation);
            Assert.Equal(personId, result.Value.Person);
            Assert.Equal(editionId, result.Value.Edition);
            Assert.Equal(registerTypeId, result.Value.RegisterType);
        }

        [Theory]
        [InlineData(-10, Participation.CLIENT, "Total must be greater than or equal to 0.")]
        [InlineData(100, Participation.EMPLOYEE, "Employees do not have to pay.")]
        [InlineData(1000, Participation.GUEST, "Guests do not have to pay.")]
        [InlineData(0, Participation.CLIENT, "Client have to pay.")]
        public void Create_WithInvalidInput_ReturnsFailure(decimal total, Participation participation, string expected)
        {
            // Arrange
            var personId = Guid.NewGuid();
            var editionId = Guid.NewGuid();
            var registerTypeId = Guid.NewGuid();

            // Act
            var result = Register.Create(total, "code", personId, editionId, registerTypeId, participation);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(total, result.Value.Total);
            Assert.Equal("code", result.Value.SponsorCode);
            Assert.Equal(participation, result.Value.Participation);
            Assert.Equal(personId, result.Value.Person);
            Assert.Equal(editionId, result.Value.Edition);
            Assert.Equal(registerTypeId, result.Value.RegisterType);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var institutionId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var personId = Guid.NewGuid();

            var address = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, address, eventId, institutionId).Value!;

            var registerTypeIntsance = RegisterType.Create("name", "description", 0, 1, editionInstance.Id).Value!;

            var registerInstance = Register.Create(registerTypeIntsance.Price, "code", personId, editionInstance.Id, registerTypeIntsance.Id, Participation.CLIENT).Value!;

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
