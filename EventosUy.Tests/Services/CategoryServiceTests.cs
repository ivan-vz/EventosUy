using EventosUy.Application.Services;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetById_IfExists_ReturnSuccess() 
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

        [Fact]
        public async Task GetAll_WhenExists_ReturnSuccess() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            var cat1 = Category.Create("Cat1", "Descr1").Value!;
            var cat2 = Category.Create("Cat2", "Descr2").Value!;

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([cat1, cat2]);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value!.Count);

            // Verify
            var card1 = result.Value[0];
            Assert.Equal(cat1.Id, card1.Id);
            Assert.Equal("Cat1", card1.Name);
            Assert.Equal("Descr1", card1.Description);
            Assert.Equal(cat1.Created, card1.Creation);

            var card2 = result.Value[1];
            Assert.Equal(cat2.Id, card2.Id);
            Assert.Equal("Cat2", card2.Name);
            Assert.Equal("Descr2", card2.Description);
            Assert.Equal(cat2.Created, card2.Creation);
        }

        [Fact]
        public async Task GetAll_WhenEmpty_ReturnSuccess() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task Create_ReturnSuccess() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            mockRepo.Setup(repo => repo.ExistsAsync("NewCategory")).ReturnsAsync(false);

            Category? capturedCategory = null;
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Category>())).Callback<Category>(c => capturedCategory = c).Returns(Task.CompletedTask);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.CreateAsync("NewCategory", "Description");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            Assert.Equal(capturedCategory!.Id, result.Value);

            // Verify
            mockRepo.Verify(repo => repo.AddAsync(It.Is<Category>(c => "NewCategory".Equals(c.Name) && "Description".Equals(c.Description))), Times.Once);
        }

        [Fact]
        public async Task Create_WithExistingName_ReturnFailure() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            mockRepo.Setup(repo => repo.ExistsAsync("ExistingCategory")).ReturnsAsync(true);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.CreateAsync("ExistingCategory", "Description");

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(Guid.Empty, result.Value);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Category already exist.", result.Errors[0]);

            // Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never);
        }
    }
}
