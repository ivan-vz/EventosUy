using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Application.Services;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Application
{
    public  class EventServiceTest
    {
        [Fact]
        public async Task Create_With_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var dtInsert = new DTInsertEvent(
                    "name",
                    "initials",
                    "description",
                    ["cat1", "cat2"],
                    Guid.NewGuid()
                );

            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtInsert.Name)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtInsert.Initials)).ReturnsAsync(true);
            mockCategoryService.Setup(s => s.ExistsAsync(dtInsert.Categories)).ReturnsAsync(false);
            mockInstitutionService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(((DTInstitution?)null, (UserCard?)null));

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(4, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Initials" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Category" && e.ErrorMessage.Contains("Not Found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Institution" && e.ErrorMessage.Contains("Not Found"));

            //Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task Create_With_Valid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var dtInsert = new DTInsertEvent(
                    "name",
                    "initials",
                    "description",
                    ["cat1", "cat2"],
                    Guid.NewGuid()
                );

            var dtIns = new DTInstitution(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "name",
                    "acronym",
                    "description",
                    "https://url.com",
                    "country",
                    "city",
                    "street",
                    "0000",
                    0,
                    DateTime.UtcNow
                );

            var cardIns = new UserCard(dtIns.Id, dtIns.Nickname, dtIns.Email);

            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtInsert.Name)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtInsert.Name)).ReturnsAsync(false);
            mockCategoryService.Setup(s => s.ExistsAsync(dtInsert.Categories)).ReturnsAsync(true);
            mockInstitutionService.Setup(s => s.GetByIdAsync(dtInsert.Institution)).ReturnsAsync((dtIns, cardIns));

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTEvent>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Initials, result.Initials);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Description, result.Description);
            Assert.Equal(dtInsert.Categories, result.Categories);
            Assert.Equal(cardIns, result.Institution);

            //Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            
            var events = new List<Event> {
                new (
                    "name 1",
                    "initials 1",
                    "description 1",
                    ["cat1", "cat2"],
                    Guid.NewGuid()
                    ),
                new (
                    "name 2",
                    "initials 2",
                    "description 2",
                    ["cat1", "cat4"],
                    Guid.NewGuid()
                    )
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(events[0].Id, result[0].Id);
            Assert.Equal(events[0].Name, result[0].Name);
            Assert.Equal(events[0].Initials, result[0].Initials);

            Assert.Equal(events[1].Id, result[1].Id);
            Assert.Equal(events[1].Name, result[1].Name);
            Assert.Equal(events[1].Initials, result[1].Initials);

            // Verify
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Empty(result);

            // Verify
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_By_Id_With_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            
            var dtInst = new DTInstitution(
                Guid.NewGuid(), 
                "nickname",
                "email@gmail.com",
                "name",
                "acronym",
                "description",
                "https://url.com",
                "country",
                "city",
                "street",
                "number",
                0,
                DateTime.UtcNow
                );

            var cardInst = new UserCard(dtInst.Id, dtInst.Nickname, dtInst.Email);

            var expected = new Event(
                    "name 1",
                    "initials 1",
                    "description 1",
                    ["cat1", "cat2"],
                    cardInst.Id
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(expected.Id)).ReturnsAsync(expected);
            mockInstitutionService.Setup(s => s.GetByIdAsync(expected.InstitutionId)).ReturnsAsync((dtInst, cardInst));

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(expected.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTEvent>(dtResult);
            Assert.IsType<EventCard>(cardResult);

            Assert.Equal(expected.Id, dtResult.Id);
            Assert.Equal(expected.Name, dtResult.Name);
            Assert.Equal(expected.Initials, dtResult.Initials);
            Assert.Equal(expected.Description, dtResult.Description);
            Assert.Equal(expected.Categories, [.. dtResult.Categories]);
            Assert.Equal(cardInst, dtResult.Institution);
            Assert.Equal(expected.Created, dtResult.Created);

            Assert.Equal(expected.Id, cardResult.Id);
            Assert.Equal(expected.Name, cardResult.Name);
            Assert.Equal(expected.Initials, cardResult.Initials);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var noExistentEvent = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event?)null);

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(noExistentEvent);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Update_When_Event_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            
            var dtUpdate = new DTUpdateEvent(
                    Guid.NewGuid(),
                    "Name",
                    "Initials",
                    "Description",
                    ["cat3", "cat6"]
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync((Event?)null);

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Single(validation.Errors);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Id" && e.ErrorMessage.Contains("not found"));
        }

        [Fact]
        public async Task Update_When_Data_Is_Invalid()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var _event = new Event(
                    "name 1",
                    "initials 1",
                    "description 1",
                    ["cat1", "cat2"],
                    Guid.NewGuid()
                    );

            var dtUpdate = new DTUpdateEvent(
                    _event.Id,
                    "Updated 1",
                    "Updated 2",
                    "Updated 3",
                    ["Upd4", "Upd5"]
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(_event);
            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtUpdate.Name)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtUpdate.Initials)).ReturnsAsync(true);
            mockCategoryService.Setup(s => s.ExistsAsync(dtUpdate.Categories)).ReturnsAsync(false);

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(3, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Initials" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Category" && e.ErrorMessage.Contains("Not Found"));

            Assert.NotEqual(_event.Name, dtUpdate.Name);
            Assert.NotEqual(_event.Initials, dtUpdate.Initials);
            Assert.NotEqual(_event.Description, dtUpdate.Description);
            Assert.NotEqual(_event.Categories, [.. dtUpdate.Categories]);
        }

        [Fact]
        public async Task Update_When_Event_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var dtInst = new DTInstitution(
                Guid.NewGuid(),
                "nickname",
                "email@gmail.com",
                "name",
                "acronym",
                "description",
                "https://url.com",
                "country",
                "city",
                "street",
                "number",
                0,
                DateTime.UtcNow
                );

            var cardInst = new UserCard(dtInst.Id, dtInst.Nickname, dtInst.Email);

            var _event = new Event(
                    "name 1",
                    "initials 1",
                    "description 1",
                    ["cat1", "cat2"],
                    dtInst.Id
                    );

            var dtUpdate = new DTUpdateEvent(
                    _event.Id,
                    "Updated 1",
                    "Updated 2",
                    "Updated 3",
                    ["Upd4", "Upd5"]
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(_event.Id)).ReturnsAsync(_event);
            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtUpdate.Name)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtUpdate.Name)).ReturnsAsync(false);
            mockCategoryService.Setup(s => s.ExistsAsync(dtUpdate.Categories)).ReturnsAsync(true);

            mockInstitutionService.Setup(s => s.GetByIdAsync(_event.InstitutionId)).ReturnsAsync((dtInst, cardInst));

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTEvent>(result);
            
            // DT created well
            Assert.Equal(_event.Id, result.Id);
            Assert.Equal(_event.Name, result.Name);
            Assert.Equal(_event.Initials, result.Initials);
            Assert.Equal(_event.Description, result.Description);
            Assert.Equal(_event.Created, result.Created);
            Assert.Equal(_event.Categories, [.. result.Categories]);
            Assert.Equal(cardInst, result.Institution);

            // Client Updated
            Assert.Equal(_event.Name, dtUpdate.Name);
            Assert.Equal(_event.Initials, dtUpdate.Initials);
            Assert.Equal(_event.Description, dtUpdate.Description);
            Assert.Equal(_event.Categories, [.. dtUpdate.Categories]);
        }

        [Fact]
        public async Task Delete_When_Event_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Event?)null);

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var result = await service.DeleteAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_When_Event_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEventRepo>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockInstitutionService = new Mock<IInstitutionService>();

            var dtInst = new DTInstitution(
                Guid.NewGuid(),
                "nickname",
                "email@gmail.com",
                "name",
                "acronym",
                "description",
                "https://url.com",
                "country",
                "city",
                "street",
                "number",
                0,
                DateTime.UtcNow
                );

            var cardInst = new UserCard(dtInst.Id, dtInst.Nickname, dtInst.Email);

            var _event = new Event(
                    "name 1",
                    "initials 1",
                    "description 1",
                    ["cat1", "cat2"],
                    dtInst.Id
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(_event.Id)).ReturnsAsync(_event);
            mockInstitutionService.Setup(s => s.GetByIdAsync(_event.InstitutionId)).ReturnsAsync((dtInst, cardInst));

            var service = new EventService(mockRepo.Object, mockCategoryService.Object, mockInstitutionService.Object);

            // Act
            var result = await service.DeleteAsync(_event.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DTEvent>(result);

            // DT created well
            Assert.Equal(_event.Id, result.Id);
            Assert.Equal(_event.Name, result.Name);
            Assert.Equal(_event.Initials, result.Initials);
            Assert.Equal(_event.Description, result.Description);
            Assert.Equal(_event.Created, result.Created);
            Assert.Equal(_event.Categories, [.. result.Categories]);
            Assert.Equal(cardInst, result.Institution);

            // Client Updated
            Assert.False(_event.Active);
        }
    }
}
