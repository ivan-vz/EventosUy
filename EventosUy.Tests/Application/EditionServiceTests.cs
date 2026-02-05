using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Interfaces;
using EventosUy.Application.Services;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Application
{
    public class EditionServiceTests
    {
        [Fact]
        public async Task Create_With_Valid_Data() 
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var dtEvent = new DTEvent(
                    Guid.NewGuid(),
                    "ename",
                    "einitials",
                    "edescription",
                    DateTime.UtcNow,
                    ["cat1", "cat2"],
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
                );

            var cardEvent = new EventCard(dtEvent.Id, dtEvent.Name, dtEvent.Initials);

            var dtInsert = new DTInsertEdition(
                    "name",
                    "initials",
                    DateOnly.Parse("2027-01-10"),
                    DateOnly.Parse("2027-01-10"),
                    "country",
                    "city",
                    "street",
                    "0000",
                    0,
                    dtEvent.Id
                );

            mockEventService.Setup(s => s.GetByIdAsync(dtInsert.Event)).ReturnsAsync((dtEvent, cardEvent));
            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtInsert.Name)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtInsert.Initials)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsEventAt(
                dtInsert.Country, 
                dtInsert.City,
                dtInsert.Street,
                dtInsert.Number,
                dtInsert.Floor,
                dtInsert.From)).ReturnsAsync(false);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTEdition>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Initials, result.Initials);
            Assert.Equal(dtInsert.From, result.From);
            Assert.Equal(dtInsert.To, result.To);
            Assert.Equal(dtInsert.Country, result.Country);
            Assert.Equal(dtInsert.City, result.City);
            Assert.Equal(dtInsert.Street, result.Street);
            Assert.Equal(dtInsert.Number, result.Number);
            Assert.Equal(dtInsert.Floor, result.Floor);
            Assert.Equal(cardEvent, result.Event);
            Assert.Equal(dtEvent.Institution, result.Institution);

            // Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Edition>()), Times.Once);
        }

        [Fact]
        public async Task Create_With_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var dtInsert = new DTInsertEdition(
                    "name",
                    "initials",
                    DateOnly.Parse("2027-01-10"),
                    DateOnly.Parse("2027-01-10"),
                    "country",
                    "city",
                    "street",
                    "0000",
                    0,
                    Guid.NewGuid()
                );

            mockEventService.Setup(s => s.GetByIdAsync(dtInsert.Event)).ReturnsAsync((null, null));
            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtInsert.Name)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtInsert.Initials)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsEventAt(
                dtInsert.Country,
                dtInsert.City,
                dtInsert.Street,
                dtInsert.Number,
                dtInsert.Floor,
                dtInsert.From)).ReturnsAsync(true);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);

            Assert.Equal(4, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Event" && e.ErrorMessage.Contains("Not Found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Initials" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Address | Date" && e.ErrorMessage.Contains("already in booked for"));
           
            // Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Edition>()), Times.Never);
        }

        [Fact]
        public async Task Get_All_When_Data_Exists() 
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var editions = new List<Edition>
            {
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    ),
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    )
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(editions);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(editions[0].Id, result[0].Id);
            Assert.Equal(editions[0].Name, result[0].Name);
            Assert.Equal(editions[0].Initials, result[0].Initials);

            Assert.Equal(editions[1].Id, result[1].Id);
            Assert.Equal(editions[1].Name, result[1].Name);
            Assert.Equal(editions[1].Initials, result[1].Initials);

            // Verify
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.Empty(result);

            // Verify
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_All_By_Event_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var eventId = Guid.NewGuid();

            var editions = new List<Edition>
            {
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        eventId,
                        Guid.NewGuid()
                    ),
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        eventId,
                        Guid.NewGuid()
                    )
            };

            mockRepo.Setup(repo => repo.GetAllByEventAsync(eventId)).ReturnsAsync(editions);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllByEventAsync(eventId)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(editions[0].Id, result[0].Id);
            Assert.Equal(editions[0].Name, result[0].Name);
            Assert.Equal(editions[0].Initials, result[0].Initials);

            Assert.Equal(editions[1].Id, result[1].Id);
            Assert.Equal(editions[1].Name, result[1].Name);
            Assert.Equal(editions[1].Initials, result[1].Initials);

            // Verify
            mockRepo.Verify(repo => repo.GetAllByEventAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task Get_All_By_Event_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var eventId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByEventAsync(eventId)).ReturnsAsync([]);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllByEventAsync(eventId)).ToList();

            // Assert
            Assert.Empty(result);

            // Verify
            mockRepo.Verify(repo => repo.GetAllByEventAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task Get_All_By_Institution_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var instId = Guid.NewGuid();

            var editions = new List<Edition>
            {
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        instId
                    ),
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        instId
                    )
            };

            mockRepo.Setup(repo => repo.GetAllByInstitutionAsync(instId)).ReturnsAsync(editions);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllByInstitutionAsync(instId)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(editions[0].Id, result[0].Id);
            Assert.Equal(editions[0].Name, result[0].Name);
            Assert.Equal(editions[0].Initials, result[0].Initials);

            Assert.Equal(editions[1].Id, result[1].Id);
            Assert.Equal(editions[1].Name, result[1].Name);
            Assert.Equal(editions[1].Initials, result[1].Initials);

            // Verify
            mockRepo.Verify(repo => repo.GetAllByInstitutionAsync(instId), Times.Once);
        }

        [Fact]
        public async Task Get_All_By_Institution_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var instId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByInstitutionAsync(instId)).ReturnsAsync([]);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllByInstitutionAsync(instId)).ToList();

            // Assert
            Assert.Empty(result);

            // Verify
            mockRepo.Verify(repo => repo.GetAllByInstitutionAsync(instId), Times.Once);
        }

        [Fact]
        public async Task Get_All_Pendding_By_Event_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var eventId = Guid.NewGuid();

            var editions = new List<Edition>
            {
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        eventId,
                        Guid.NewGuid()
                    ),
                new (
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        eventId,
                        Guid.NewGuid()
                    )
            };

            mockRepo.Setup(repo => repo.GetAllPendingByEventAsync(eventId)).ReturnsAsync(editions);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllPendingByEventAsync(eventId)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(editions[0].Id, result[0].Id);
            Assert.Equal(editions[0].Name, result[0].Name);
            Assert.Equal(editions[0].Initials, result[0].Initials);

            Assert.Equal(editions[1].Id, result[1].Id);
            Assert.Equal(editions[1].Name, result[1].Name);
            Assert.Equal(editions[1].Initials, result[1].Initials);

            // Verify
            mockRepo.Verify(repo => repo.GetAllPendingByEventAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task Get_All_Pendding_By_Event_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var eventId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllPendingByEventAsync(eventId)).ReturnsAsync([]);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = (await service.GetAllPendingByEventAsync(eventId)).ToList();

            // Assert
            Assert.Empty(result);

            // Verify
            mockRepo.Verify(repo => repo.GetAllPendingByEventAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id() 
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var nonExistentId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistentId)).ReturnsAsync((Edition?)null);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(nonExistentId);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Get_By_Id_With_Pendding_Edition()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var edition = new Edition(
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(edition.Id);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Get_By_Id_With_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var dtEvent = new DTEvent(
                    Guid.NewGuid(),
                    "ename",
                    "einitials",
                    "edescription",
                    DateTime.UtcNow,
                    ["cat1", "cat2"],
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
                );

            var cardEvent = new EventCard(dtEvent.Id, dtEvent.Name, dtEvent.Initials);

            var edition = new Edition(
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        dtEvent.Id,
                        dtEvent.Id
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);
            mockEventService.Setup(s => s.GetByIdAsync(edition.Event)).ReturnsAsync((dtEvent, cardEvent));

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            await service.ApproveAsync(edition.Id);
            var (dtResult, cardResult) = await service.GetByIdAsync(edition.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTEdition>(dtResult);
            Assert.IsType<EditionCard>(cardResult);

            Assert.Equal(edition.Id, dtResult.Id);
            Assert.Equal(edition.Name, dtResult.Name);
            Assert.Equal(edition.Initials, dtResult.Initials);
            Assert.Equal(edition.From, dtResult.From);
            Assert.Equal(edition.To, dtResult.To);
            Assert.Equal(edition.Created, dtResult.Created);
            Assert.Equal(edition.State, dtResult.State);
            Assert.Equal(edition.Country, dtResult.Country);
            Assert.Equal(edition.City, dtResult.City);
            Assert.Equal(edition.Street, dtResult.Street);
            Assert.Equal(edition.Number, dtResult.Number);
            Assert.Equal(edition.Floor, dtResult.Floor);
            Assert.Equal(cardEvent, dtResult.Event);
            Assert.Equal(dtEvent.Institution, dtResult.Institution);

            Assert.Equal(edition.Id, cardResult.Id);
            Assert.Equal(edition.Name, cardResult.Name);
            Assert.Equal(edition.Initials, cardResult.Initials);
            Assert.Equal(edition.State, cardResult.State);
        }

        [Fact]
        public async Task Approve_With_Non_Existent_Id() 
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Edition?)null);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result =  await service.ApproveAsync(id);

            Assert.False(result);
        }

        [Fact]
        public async Task Approve_When_Data_Is_Invalid()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var edition = new Edition(
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    );

            edition.State = EditionState.CANCELLED;

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.ApproveAsync(edition.Id);

            Assert.False(result);
            Assert.Equal(EditionState.CANCELLED, edition.State);
        }

        [Fact]
        public async Task Approve_When_Data_Is_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var edition = new Edition(
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.ApproveAsync(edition.Id);

            Assert.True(result);
            Assert.Equal(EditionState.ONGOING, edition.State);
        }

        [Fact]
        public async Task Reject_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Edition?)null);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.RejectAsync(id);

            Assert.False(result);
        }

        [Fact]
        public async Task Reject_When_Data_Is_Invalid()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var edition = new Edition(
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    );

            edition.State = EditionState.ONGOING;

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.RejectAsync(edition.Id);

            Assert.False(result);
            Assert.Equal(EditionState.ONGOING, edition.State);
        }

        [Fact]
        public async Task Reject_When_Data_Is_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var edition = new Edition(
                        "name 1",
                        "initials 1",
                        DateOnly.Parse("2027-01-02"),
                        DateOnly.Parse("2027-01-08"),
                        "country 1",
                        "city 1",
                        "street 1",
                        "0000",
                        0,
                        Guid.NewGuid(),
                        Guid.NewGuid()
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.RejectAsync(edition.Id);

            Assert.True(result);
            Assert.Equal(EditionState.CANCELLED, edition.State);
        }

        [Fact]
        public async Task Update_With_Non_Existent_Id() 
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var dtUpdate = new DTUpdateEdition(
                    Guid.NewGuid(),
                    "name",
                    "initials",
                    DateOnly.Parse("2029-10-15"),
                    DateOnly.Parse("2029-11-10"),
                    "country",
                    "city",
                    "street",
                    "0000",
                    0
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync((Edition?)null);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

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
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var edition = new Edition(
                "name",
                "initials",
                DateOnly.Parse("2029-10-15"),
                DateOnly.Parse("2029-11-10"),
                "country",
                "city",
                "street",
                "0000",
                0,
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            edition.State = EditionState.ONGOING;

            var dtUpdate = new DTUpdateEdition(
                    edition.Id,
                    "updated",
                    "updated",
                    DateOnly.Parse("2029-10-16"),
                    DateOnly.Parse("2029-11-15"),
                    "updated",
                    "updated",
                    "updated",
                    "1111",
                    1
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(edition);
            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtUpdate.Name)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtUpdate.Initials)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsEventAt(
                dtUpdate.Country,
                dtUpdate.City,
                dtUpdate.Street,
                dtUpdate.Number,
                dtUpdate.Floor,
                dtUpdate.From
                )).ReturnsAsync(true);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(3, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Initials" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Address | Date" && e.ErrorMessage.Contains("already in booked for"));
        }

        [Fact]
        public async Task Update_When_Data_Is_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var dtEvent = new DTEvent(
                    Guid.NewGuid(),
                    "ename",
                    "einitials",
                    "edescription",
                    DateTime.UtcNow,
                    ["cat1", "cat2"],
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
                );

            var cardEvent = new EventCard(dtEvent.Id, dtEvent.Name, dtEvent.Initials);

            var edition = new Edition(
                "name",
                "initials",
                DateOnly.Parse("2029-10-15"),
                DateOnly.Parse("2029-11-10"),
                "country",
                "city",
                "street",
                "0000",
                0,
                dtEvent.Id,
                Guid.NewGuid()
            );

            edition.State = EditionState.ONGOING;

            var dtUpdate = new DTUpdateEdition(
                    edition.Id,
                    "updated",
                    "updated",
                    DateOnly.Parse("2029-10-16"),
                    DateOnly.Parse("2029-11-15"),
                    "updated",
                    "updated",
                    "updated",
                    "1111",
                    1
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(edition);
            mockRepo.Setup(repo => repo.ExistsByNameAsync(dtUpdate.Name)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByInitialsAsync(dtUpdate.Initials)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsEventAt(
                dtUpdate.Country,
                dtUpdate.City,
                dtUpdate.Street,
                dtUpdate.Number,
                dtUpdate.Floor,
                dtUpdate.From
                )).ReturnsAsync(false);
            mockEventService.Setup(s => s.GetByIdAsync(dtEvent.Id)).ReturnsAsync((dtEvent, cardEvent));

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTEdition>(result);

            // DT created well
            Assert.Equal(edition.Id, result.Id);
            Assert.Equal(edition.Name, result.Name);
            Assert.Equal(edition.Initials, result.Initials);
            Assert.Equal(edition.From, result.From);
            Assert.Equal(edition.To, result.To);
            Assert.Equal(edition.Created, result.Created);
            Assert.Equal(edition.State, result.State);
            Assert.Equal(edition.Country, result.Country);
            Assert.Equal(edition.City, result.City);
            Assert.Equal(edition.Street, result.Street);
            Assert.Equal(edition.Number, result.Number);
            Assert.Equal(edition.Floor, result.Floor);
            Assert.Equal(cardEvent, result.Event);
            Assert.Equal(dtEvent.Institution, result.Institution);

            // Client Updated
            Assert.Equal(edition.Name, dtUpdate.Name);
            Assert.Equal(edition.Initials, dtUpdate.Initials);
            Assert.Equal(edition.From, dtUpdate.From);
            Assert.Equal(edition.To, dtUpdate.To);
            Assert.Equal(edition.Name, dtUpdate.Name);
            Assert.Equal(edition.Country, dtUpdate.Country);
            Assert.Equal(edition.City, dtUpdate.City);
            Assert.Equal(edition.Street, dtUpdate.Street);
            Assert.Equal(edition.Number, dtUpdate.Number);
            Assert.Equal(edition.Floor, dtUpdate.Floor);
        }

        [Fact]
        public async Task Delete_When_Event_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Edition?)null);

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.DeleteAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_When_Event_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IEditionRepo>();
            var mockEventService = new Mock<IEventService>();

            var dtEvent = new DTEvent(
                   Guid.NewGuid(),
                   "ename",
                   "einitials",
                   "edescription",
                   DateTime.UtcNow,
                   ["cat1", "cat2"],
                   new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com")
               );

            var cardEvent = new EventCard(dtEvent.Id, dtEvent.Name, dtEvent.Initials);

            var edition = new Edition(
                "name",
                "initials",
                DateOnly.Parse("2029-10-15"),
                DateOnly.Parse("2029-11-10"),
                "country",
                "city",
                "street",
                "0000",
                0,
                dtEvent.Id,
                Guid.NewGuid()
            );

            edition.State = EditionState.ONGOING;

            mockRepo.Setup(repo => repo.GetByIdAsync(edition.Id)).ReturnsAsync(edition);
            mockEventService.Setup(s => s.GetByIdAsync(dtEvent.Id)).ReturnsAsync((dtEvent, cardEvent));

            var service = new EditionService(mockRepo.Object, mockEventService.Object);

            // Act
            var result = await service.DeleteAsync(edition.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DTEdition>(result);

            // DT created well
            Assert.Equal(edition.Id, result.Id);
            Assert.Equal(edition.Name, result.Name);
            Assert.Equal(edition.Initials, result.Initials);
            Assert.Equal(edition.From, result.From);
            Assert.Equal(edition.To, result.To);
            Assert.Equal(edition.Created, result.Created);
            Assert.Equal(edition.State, result.State);
            Assert.Equal(edition.Country, result.Country);
            Assert.Equal(edition.City, result.City);
            Assert.Equal(edition.Street, result.Street);
            Assert.Equal(edition.Number, result.Number);
            Assert.Equal(edition.Floor, result.Floor);
            Assert.Equal(cardEvent, result.Event);
            Assert.Equal(dtEvent.Institution, result.Institution);

            // Client Updated
            Assert.Equal(EditionState.CANCELLED, edition.State);
        }
    }
}
