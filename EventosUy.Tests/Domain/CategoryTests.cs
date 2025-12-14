using EventosUy.Domain.Entities;

namespace EventosUy.Tests.Domain
{
    public class CategoryTests
    {
        [Fact]
        public void Create_WithValidInput_RetrurnSuccess()
        {
            // Act
            var result = Category.Create("NewCategory", "Description");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("NewCategory", result.Value.Name);
            Assert.Equal("Description", result.Value.Description);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void Create_WithInvalidInput_ReturnFailure(string name, string desc) 
        {
            // Act
            var result = Category.Create(name, desc);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Value);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("Name can not be empty.", result.Errors);
            Assert.Contains("Description can not be empty.", result.Errors);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData() 
        {
            // Arrange
            var category1 = Category.Create("category1", "descr1").Value!;

            // Act
            var card = category1.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(category1.Id, card.Id);
            Assert.Equal("category1", card.Name);
            Assert.Equal("descr1", card.Description);
            Assert.Equal(category1.Created, card.Creation);
        }
    }
}
