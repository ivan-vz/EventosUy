using EventosUy.Application.Services;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetById_IfExists_ReturnSucces() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var expectedCategory = Category.Create("categoryOne", "DescriptionOne");

            Guid categoryId = expectedCategory.Value!.Id;

            mockRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(expectedCategory.Value);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(categoryId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("categoryOne", result.Value.Name);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task GetById_IfNotExists_ReturnFailure()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var noExistentCategory = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(noExistentCategory);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Value);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Category not Found.", result.Errors[0]);
        }

        [Fact]
        public async Task GetById_WithEmptyGuid_ReturnFailure()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            mockRepo.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync((Category?)null);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(Guid.Empty);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Value);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Category can not be empty.", result.Errors[0]);

            // Verify
            mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
