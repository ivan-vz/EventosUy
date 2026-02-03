using EventosUy.Application.DTOs.DataTypes.Detail;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.DTOs.Records;
using EventosUy.Application.Services;
using EventosUy.Domain.Common;
using EventosUy.Domain.Entities;
using EventosUy.Domain.Interfaces;
using Moq;

namespace EventosUy.Tests.Application
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task Create_With_Invalid_Data() 
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();
            
            var dtInsert = new DTInsertClient(
                    "nickname",
                    "Password0123",
                    "email@gmail.com",
                    DateOnly.Parse("2004-08-02"),
                    "0123456",
                    "FName",
                    null,
                    "FSurname",
                    "LSurname"
                );

            mockRepo.Setup(repo => repo.ExistsByNicknameAsync(dtInsert.Nickname)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByEmailAsync(dtInsert.Email)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByCiAsync(dtInsert.Ci)).ReturnsAsync(true);

            var service = new ClientService(mockRepo.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(3, validation.Errors.Count());
            Assert.Contains(validation.Errors, e => e.PropertyName == "Email" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Nickname" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Ci" && e.ErrorMessage.Contains("already in use"));

            //Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task Create_With_Valid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();

            var dtInsert = new DTInsertClient(
                    "nickname",
                    "Password0123",
                    "email@gmail.com",
                    DateOnly.Parse("2004-08-02"),
                    "0123456",
                    "FName",
                    null,
                    "FSurname",
                    "LSurname"
                );

            mockRepo.Setup(repo => repo.ExistsByNicknameAsync(dtInsert.Nickname)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByEmailAsync(dtInsert.Email)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByCiAsync(dtInsert.Ci)).ReturnsAsync(false);

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);

            var service = new ClientService(mockRepo.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTClient>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Nickname, result.Nickname);
            Assert.Equal(dtInsert.Email, result.Email);
            Assert.Equal(dtInsert.FirstName, result.FirstName);
            Assert.Equal(dtInsert.Birthday, result.Birthday);
            Assert.Equal(dtInsert.Ci, result.Ci);

            //Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Data_Exists() 
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();
            var clients = new List<Client> {
                new Client("nickname 1", "Password1234", "email1@gmail.com", "FName1", null, "FSurname1", "LSurname1", DateOnly.Parse("2003-04-07"), "0123456"),
                new Client("nickname 2", "Password1234", "email2@gmail.com", "FName2", null, "FSurname2", "LSurname2", DateOnly.Parse("2003-02-17"), "7894561")
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(clients);

            var service = new ClientService(mockRepo.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(clients[0].Id, result[0].Id);
            Assert.Equal(clients[0].Nickname, result[0].Nickname);
            Assert.Equal(clients[0].Email, result[0].Email);

            Assert.Equal(clients[1].Id, result[1].Id);
            Assert.Equal(clients[1].Nickname, result[1].Nickname);
            Assert.Equal(clients[1].Email, result[1].Email);

            // Verify
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            var service = new ClientService(mockRepo.Object);

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
            var mockRepo = new Mock<IClientRepo>();

            var expected = new Client(
                "nickname 1", 
                "Password1234", 
                "email1@gmail.com", 
                "FName1", 
                null, 
                "FSurname1", 
                "LSurname1", 
                DateOnly.Parse("2003-04-07"), 
                "0123456"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

            var service = new ClientService(mockRepo.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(expected.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTClient>(dtResult);
            Assert.IsType<UserCard>(cardResult);

            Assert.Equal(expected.Id, dtResult.Id);
            Assert.Equal(expected.Nickname, dtResult.Nickname);
            Assert.Equal(expected.Email, dtResult.Email);
            Assert.Equal(expected.FirstName, dtResult.FirstName);
            Assert.Equal(expected.LastName, dtResult.LastName);
            Assert.Equal(expected.FirstSurname, dtResult.FirstSurname);
            Assert.Equal(expected.Birthday, dtResult.Birthday);
            Assert.Equal(expected.Created, dtResult.Created);
            Assert.Equal(expected.Ci, dtResult.Ci);

            Assert.Equal(expected.Id, cardResult.Id);
            Assert.Equal(expected.Nickname, cardResult.Nickname);
            Assert.Equal(expected.Email, cardResult.Email);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();
            var noExistentClient = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Client?)null);

            var service = new ClientService(mockRepo.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(noExistentClient);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Update_When_Client_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();

            var dtUpdate = new DTUpdateClient(
                    Guid.NewGuid(),
                    "nickname",
                    "Password0123",
                    "email@gmail.com"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync((Client?)null);

            var service = new ClientService(mockRepo.Object);

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
            var mockRepo = new Mock<IClientRepo>();

            var client = new Client(
                "nickname 1",
                "Password1234",
                "email1@gmail.com",
                "FName1",
                null,
                "FSurname1",
                "LSurname1",
                DateOnly.Parse("2003-04-07"),
                "0123456"
                );

            var dtUpdate = new DTUpdateClient(
                    client.Id,
                    "nickname",
                    "Password0123",
                    "email@gmail.com"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(client);
            mockRepo.Setup(repo => repo.ExistsByNicknameAsync(dtUpdate.Nickname)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByEmailAsync(dtUpdate.Email)).ReturnsAsync(true);

            var service = new ClientService(mockRepo.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count());
            Assert.Contains(validation.Errors, e => e.PropertyName == "Nickname" && e.ErrorMessage.Contains("is already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Email" && e.ErrorMessage.Contains("is already in use"));

            Assert.NotEqual(client.Nickname, dtUpdate.Nickname);
            Assert.NotEqual(client.Email, dtUpdate.Email);
            Assert.False(PasswordHasher.Verify(dtUpdate.Password, client.Password));
        }

        [Fact]
        public async Task Update_When_Client_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();

            var client = new Client(
                "nickname 1",
                "Password1234",
                "email1@gmail.com",
                "FName1",
                null,
                "FSurname1",
                "LSurname1",
                DateOnly.Parse("2003-04-07"),
                "0123456"
                );

            var dtUpdate = new DTUpdateClient(
                    client.Id,
                    "Updated",
                    "Updated01234",
                    "updated@gmail.com"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(client);

            var service = new ClientService(mockRepo.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTClient>(result);

            // DT created well
            Assert.Equal(client.Id, result.Id);
            Assert.Equal(client.Nickname, result.Nickname);
            Assert.Equal(client.Email, result.Email);
            Assert.Equal(client.FirstName, result.FirstName);
            Assert.Equal(client.LastName, result.LastName);
            Assert.Equal(client.FirstSurname, result.FirstSurname);
            Assert.Equal(client.Birthday, result.Birthday);
            Assert.Equal(client.Created, result.Created);
            Assert.Equal(client.Ci, result.Ci);

            // Client Updated
            Assert.Equal(client.Nickname, dtUpdate.Nickname);
            Assert.True(PasswordHasher.Verify(dtUpdate.Password, client.Password));
            Assert.Equal(client.Email, dtUpdate.Email);
        }

        [Fact]
        public async Task Delete_When_Client_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Client?)null);

            var service = new ClientService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_When_Client_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepo>();

            var client = new Client(
                "nickname 1",
                "Password1234",
                "email1@gmail.com",
                "FName1",
                null,
                "FSurname1",
                "LSurname1",
                DateOnly.Parse("2003-04-07"),
                "0123456"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(client.Id)).ReturnsAsync(client);

            var service = new ClientService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(client.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DTClient>(result);

            // DT created well
            Assert.Equal(client.Id, result.Id);
            Assert.Equal(client.Nickname, result.Nickname);
            Assert.Equal(client.Email, result.Email);
            Assert.Equal(client.FirstName, result.FirstName);
            Assert.Equal(client.LastName, result.LastName);
            Assert.Equal(client.FirstSurname, result.FirstSurname);
            Assert.Equal(client.Birthday, result.Birthday);
            Assert.Equal(client.Created, result.Created);
            Assert.Equal(client.Ci, result.Ci);

            // Client Updated
            Assert.False(client.Active);
        }
    }
}
