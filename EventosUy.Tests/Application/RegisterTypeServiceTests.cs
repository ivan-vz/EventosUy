using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Application.Services;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Application
{
    public class RegisterTypeServiceTests
    {

         
         [Fact]
        public async Task Create_When_Data_Is_Valid() 
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();
            
            var dtEdition = new DTEdition(
                    Guid.NewGuid(),
                    "ename",
                    "einitials",
                    DateOnly.Parse("2027-10-10"),
                    DateOnly.Parse("2027-11-01"),
                    DateTime.UtcNow,
                    EditionState.ONGOING,
                    "country",
                    "city",
                    "street",
                    "0000",
                    1,
                    new EventCard(Guid.NewGuid(), "evname", "evinitials"),
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
                );

            var cardEdition = new EditionCard(dtEdition.Id, dtEdition.Name, dtEdition.Initials, dtEdition.State);

            var dtInsert = new DTInsertRegisterType(
                    "name",
                    "description",
                    0,
                    1,
                    dtEdition.Id
                );

            mockRepo.Setup(repo => repo.ExistsAsync(dtInsert.Name)).ReturnsAsync(false);
            mockEditionService.Setup(s => s.GetByIdAsync(dtInsert.Edition)).ReturnsAsync((dtEdition, cardEdition));

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTRegisterType>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Description, result.Description);
            Assert.Equal(dtInsert.Price, result.Price);
            Assert.Equal(dtInsert.Quota, result.Quota);
            Assert.True(result.Active);
            Assert.Equal(cardEdition, result.Edition);
        }

        [Fact]
        public async Task Create_When_Data_Is_Invalid() 
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var dtInsert = new DTInsertRegisterType(
                    "name",
                    "description",
                    0,
                    1,
                    Guid.NewGuid()
                );

            mockRepo.Setup(repo => repo.ExistsAsync(dtInsert.Name)).ReturnsAsync(true);
            mockEditionService.Setup(s => s.GetByIdAsync(dtInsert.Edition)).ReturnsAsync((null, null));

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count);

            Assert.Contains(validation.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Edition" && e.ErrorMessage.Contains("not found"));
        }

        [Fact]
        public async Task Get_All_When_Empty() 
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var nonExistentId= Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByEditionAsync(nonExistentId)).ReturnsAsync([]);

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var result = (await service.GetAllByEditionAsync(nonExistentId)).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_All_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var registerTypes = new List<RegisterType>()
            {//string name, string description, decimal price, int quota, Guid editionId
                new ("name 1", "description 1", 0, 1, Guid.NewGuid()),
                new ("name 2", "description 2", 1, 2, Guid.NewGuid())
            };

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByEditionAsync(id)).ReturnsAsync(registerTypes);

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var result = (await service.GetAllByEditionAsync(id)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(registerTypes[0].Id, result[0].Id);
            Assert.Equal(registerTypes[0].Name, result[0].Name);
            Assert.Equal(registerTypes[0].Price, result[0].Price);
            Assert.Equal(registerTypes[0].Quota, result[0].Quota);

            Assert.Equal(registerTypes[1].Id, result[1].Id);
            Assert.Equal(registerTypes[1].Name, result[1].Name);
            Assert.Equal(registerTypes[1].Price, result[1].Price);
            Assert.Equal(registerTypes[1].Quota, result[1].Quota);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var nonExistentId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistentId)).ReturnsAsync((RegisterType?)null);

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(nonExistentId);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Get_By_Id_With_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var dtEdition = new DTEdition(
                    Guid.NewGuid(),
                    "ename",
                    "einitials",
                    DateOnly.Parse("2027-10-10"),
                    DateOnly.Parse("2027-11-01"),
                    DateTime.UtcNow,
                    EditionState.ONGOING,
                    "country",
                    "city",
                    "street",
                    "0000",
                    1,
                    new EventCard(Guid.NewGuid(), "evname", "evinitials"),
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
                );

            var cardEdition = new EditionCard(dtEdition.Id, dtEdition.Name, dtEdition.Initials, dtEdition.State);
            
            var registerType = new RegisterType(
                    "name",
                    "description",
                    0, 
                    1, 
                    cardEdition.Id
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(registerType.Id)).ReturnsAsync(registerType);
            mockEditionService.Setup(s => s.GetByIdAsync(registerType.Edition)).ReturnsAsync((dtEdition, cardEdition));

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var(dtResult, cardResult) = await service.GetByIdAsync(registerType.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardEdition);
            Assert.IsType<DTRegisterType>(dtResult);
            Assert.IsType<RegisterTypeCard>(cardResult);

            Assert.Equal(registerType.Id, dtResult.Id);
            Assert.Equal(registerType.Name, dtResult.Name);
            Assert.Equal(registerType.Description, dtResult.Description);
            Assert.Equal(registerType.Price, dtResult.Price);
            Assert.Equal(registerType.Quota, dtResult.Quota);
            Assert.Equal(registerType.Used, dtResult.Used);
            Assert.Equal(cardEdition, dtResult.Edition);

            Assert.Equal(registerType.Id, cardResult.Id);
            Assert.Equal(registerType.Name, cardResult.Name);
            Assert.Equal(registerType.Price, cardResult.Price);
            Assert.Equal(registerType.Quota, cardResult.Quota);
        }

        [Fact]
        public async Task Delete_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var nonExistentId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistentId)).ReturnsAsync((RegisterType?)null);

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var result = await service.DeleteAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }
       
        [Fact]
        public async Task Delete_With_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var dtEdition = new DTEdition(
                    Guid.NewGuid(),
                    "ename",
                    "einitials",
                    DateOnly.Parse("2027-10-10"),
                    DateOnly.Parse("2027-11-01"),
                    DateTime.UtcNow,
                    EditionState.ONGOING,
                    "country",
                    "city",
                    "street",
                    "0000",
                    1,
                    new EventCard(Guid.NewGuid(), "evname", "evinitials"),
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
                );

            var cardEdition = new EditionCard(dtEdition.Id, dtEdition.Name, dtEdition.Initials, dtEdition.State);

            var registerType = new RegisterType(
                    "name",
                    "description",
                    0,
                    1,
                    cardEdition.Id
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(registerType.Id)).ReturnsAsync(registerType);
            mockEditionService.Setup(s => s.GetByIdAsync(registerType.Edition)).ReturnsAsync((dtEdition, cardEdition));

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            var result = await service.DeleteAsync(registerType.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(cardEdition);
            Assert.IsType<DTRegisterType>(result);

            Assert.Equal(registerType.Id, result.Id);
            Assert.Equal(registerType.Name, result.Name);
            Assert.Equal(registerType.Description, result.Description);
            Assert.Equal(registerType.Price, result.Price);
            Assert.Equal(registerType.Quota, result.Quota);
            Assert.Equal(registerType.Used, result.Used);
            Assert.Equal(cardEdition, result.Edition);

            Assert.False(registerType.Active);
        }

        [Fact]
        public async Task Use_Spot()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var registerType = new RegisterType(
                    "name",
                    "description",
                    0,
                    3,
                    Guid.NewGuid()
                );

            var used = registerType.Used;

            mockRepo.Setup(repo => repo.GetByIdAsync(registerType.Id)).ReturnsAsync(registerType);

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            await service.UseSpotAsync(registerType.Id);

            // Assert
            Assert.NotEqual(registerType.Quota, registerType.Used);
            Assert.True((used + 1) == registerType.Used);
            Assert.True(registerType.Active);
        }

        [Fact]
        public async Task Use_All_Spots()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterTypeRepo>();
            var mockEditionService = new Mock<IEditionService>();

            var registerType = new RegisterType(
                    "name",
                    "description",
                    0,
                    1,
                    Guid.NewGuid()
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(registerType.Id)).ReturnsAsync(registerType);

            var service = new RegisterTypeService(mockRepo.Object, mockEditionService.Object);

            // Act
            await service.UseSpotAsync(registerType.Id);

            // Assert
            Assert.Equal(registerType.Quota, registerType.Used);
            Assert.False(registerType.Active);
        }

    }
}
