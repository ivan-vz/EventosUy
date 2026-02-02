using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.Services;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Application
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task Get_By_Id_With_Existent_Id() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            var name = "name";
            var expected = new Category(name);

            mockRepo.Setup(repo => repo.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(expected.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DTCategory>(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(name, result.Name);
            Assert.Equal(expected.Created, result.Created);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var noExistentCategory = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(noExistentCategory);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Get_All_When_Exists() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            var cat1 = new Category("Cat1");
            var cat2 = new Category("Cat2");

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([cat1, cat2]);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(cat1.Name, result);
            Assert.Contains(cat1.Name, result);
        }

        [Fact]
        public async Task Get_All_When_Empty() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Exists_Async_When_All_Names_Exists() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var names = new[] { "cat1", "cat2", "cat3" };

            mockRepo.Setup(repo => repo.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.ExistsAsync(names);

            // Assert
            Assert.True(result);

            // Verify
            mockRepo.Verify(repo => repo.ExistsAsync(It.IsAny<string>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Exists_Async_When_Not_All_Names_Exists()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var names = new[] { "cat1", "cat2", "cat3" };

            mockRepo.Setup(repo => repo.ExistsAsync("cat1")).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsAsync("cat2")).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsAsync("cat3")).ReturnsAsync(true);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.ExistsAsync(names);

            // Assert
            Assert.False(result);

            // Verify
            mockRepo.Verify(repo => repo.ExistsAsync(It.IsAny<string>()), Times.Exactly(2));
            mockRepo.Verify(repo => repo.ExistsAsync("cat3"), Times.Never);
        }

        [Fact]
        public async Task Create_With_valid_data() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var name = "name";

            mockRepo.Setup(repo => repo.ExistsAsync(name)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var (result, validation) = await service.CreateAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(name, result.Name);

            // Verify
            mockRepo.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == name )), Times.Once);
        }

        [Fact]
        public async Task Create_With_Existing_Name() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var name = "name";

            mockRepo.Setup(repo => repo.ExistsAsync(name)).ReturnsAsync(true);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var (result, validation) = await service.CreateAsync(name);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Single(validation.Errors);
            Assert.Equal("Name", validation.Errors[0].PropertyName);
            Assert.Contains("Name is already in use.", validation.Errors[0].ErrorMessage);

            // Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task Delete_With_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var name = "name";
            var category = new Category(name);

            mockRepo.Setup(repo => repo.GetByIdAsync(category.Id)).ReturnsAsync(category);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.Created, result.Created);
            Assert.False(category.Active);
        }

        [Fact]
        public async Task Delete_With_Non_Existent_Id() 
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Category?)null);

            var service = new CategoryService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(id);

            // Assert
            Assert.Null(result);
        }
    }
}
