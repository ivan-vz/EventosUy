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
    public class InstitutionServiceTests
    {
        [Fact]
        public async Task Create_With_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();
            
            var dtInsert = new DTInsertInstitution(
                    "nickname",
                    "Password0123",
                    "email@gmail.com",
                    "name",
                    "acronym",
                    "description",
                    "https://url.com",
                    "country",
                    "city",
                    "street",
                    "0000",
                    0
                );

            mockRepo.Setup(repo => repo.ExistsByNicknameAsync(dtInsert.Nickname)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByEmailAsync(dtInsert.Email)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByAcronymAsync(dtInsert.Acronym)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByUrlAsync(dtInsert.Url)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByAddressAsync(
                dtInsert.Country,
                dtInsert.City,
                dtInsert.Street,
                dtInsert.Number,
                dtInsert.Floor
                )).ReturnsAsync(true);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(5, validation.Errors.Count());
            Assert.Contains(validation.Errors, e => e.PropertyName == "Nickname" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Email" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Acronym" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Url" && e.ErrorMessage.Contains("already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Address" && e.ErrorMessage.Contains("already in use"));

            //Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Institution>()), Times.Never);
        }

        [Fact]
        public async Task Create_With_Valid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();

            var dtInsert = new DTInsertInstitution(
                    "nickname",
                    "Password0123",
                    "email@gmail.com",
                    "name",
                    "acronym",
                    "description",
                    "https://url.com",
                    "country",
                    "city",
                    "street",
                    "0000",
                    0
                );

            mockRepo.Setup(repo => repo.ExistsByNicknameAsync(dtInsert.Nickname)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByEmailAsync(dtInsert.Email)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByAcronymAsync(dtInsert.Acronym)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByUrlAsync(dtInsert.Url)).ReturnsAsync(false);
            mockRepo.Setup(repo => repo.ExistsByAddressAsync(
                dtInsert.Country,
                dtInsert.City,
                dtInsert.Street,
                dtInsert.Number,
                dtInsert.Floor
                )).ReturnsAsync(false);

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Institution>())).Returns(Task.CompletedTask);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTInstitution>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Nickname, result.Nickname);
            Assert.Equal(dtInsert.Email, result.Email);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Acronym, result.Acronym);
            Assert.Equal(dtInsert.Description, result.Description);
            Assert.Equal(dtInsert.Url, result.Url);
            Assert.Equal(dtInsert.Country, result.Country);
            Assert.Equal(dtInsert.City, result.City);
            Assert.Equal(dtInsert.Street, result.Street);
            Assert.Equal(dtInsert.Number, result.Number);
            Assert.Equal(dtInsert.Floor, result.Floor);

            //Verify
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Institution>()), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();
            var institution = new List<Institution> {
                new Institution(
                    "nickname 1", 
                    "Password1234", 
                    "Acronym 1",
                    "email1@gmail.com", 
                    "name 1",
                    "description 1", 
                    "https://urlUno.com", 
                    "country 1", 
                    "city 1", 
                    "street 1",
                    "0000", 
                    1
                    ),
                new Institution(
                    "nickname 2",
                    "Password1434",
                    "Acronym 2",
                    "email2@gmail.com",
                    "name 2",
                    "description 2",
                    "https://urlDos.com",
                    "country 2",
                    "city 2",
                    "street 2",
                    "1111",
                    0
                    ),
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(institution);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(institution[0].Id, result[0].Id);
            Assert.Equal(institution[0].Nickname, result[0].Nickname);
            Assert.Equal(institution[0].Email, result[0].Email);

            Assert.Equal(institution[1].Id, result[1].Id);
            Assert.Equal(institution[1].Nickname, result[1].Nickname);
            Assert.Equal(institution[1].Email, result[1].Email);

            // Verify
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_All_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            var service = new InstitutionService(mockRepo.Object);

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
            var mockRepo = new Mock<IInstitutionRepo>();

            var expected = new Institution(
                    "nickname 1",
                    "Password1234",
                    "Acronym 1",
                    "email1@gmail.com",
                    "name 1",
                    "description 1",
                    "https://urlUno.com",
                    "country 1",
                    "city 1",
                    "street 1",
                    "0000",
                    1
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(expected.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTInstitution>(dtResult);
            Assert.IsType<UserCard>(cardResult);

            Assert.Equal(expected.Id, dtResult.Id);
            Assert.Equal(expected.Nickname, dtResult.Nickname);
            Assert.Equal(expected.Email, dtResult.Email);
            Assert.Equal(expected.Name, dtResult.Name);
            Assert.Equal(expected.Acronym, dtResult.Acronym);
            Assert.Equal(expected.Url, dtResult.Url);
            Assert.Equal(expected.Description, dtResult.Description);
            Assert.Equal(expected.Country, dtResult.Country);
            Assert.Equal(expected.City, dtResult.City);
            Assert.Equal(expected.Street, dtResult.Street);
            Assert.Equal(expected.Number, dtResult.Number);
            Assert.Equal(expected.Floor, dtResult.Floor);
            Assert.Equal(expected.Created, dtResult.Created);

            Assert.Equal(expected.Id, cardResult.Id);
            Assert.Equal(expected.Nickname, cardResult.Nickname);
            Assert.Equal(expected.Email, cardResult.Email);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();
            var noExistentClient = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Institution?)null);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(noExistentClient);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Update_When_Institution_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();

            var dtUpdate = new DTUpdateInstitution(
                    Guid.NewGuid(),
                    "nickname",
                    "Password0123",
                    "email@gmail.com",
                    "description",
                    "https://urlUpdated.com"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync((Institution?)null);

            var service = new InstitutionService(mockRepo.Object);

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
            var mockRepo = new Mock<IInstitutionRepo>();

            var institution = new Institution(
                    "nickname 1",
                    "Password1234",
                    "Acronym 1",
                    "email1@gmail.com",
                    "name 1",
                    "description 1",
                    "https://urlUno.com",
                    "country 1",
                    "city 1",
                    "street 1",
                    "0000",
                    1
                    );

            var dtUpdate = new DTUpdateInstitution(
                    Guid.NewGuid(),
                    "Updated",
                    "Updated01234",
                    "updated@gmail.com",
                    "Updated",
                    "https://Updated.com"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(institution);
            mockRepo.Setup(repo => repo.ExistsByNicknameAsync(dtUpdate.Nickname)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByEmailAsync(dtUpdate.Email)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.ExistsByUrlAsync(dtUpdate.Url)).ReturnsAsync(true);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(3, validation.Errors.Count());
            Assert.Contains(validation.Errors, e => e.PropertyName == "Nickname" && e.ErrorMessage.Contains("is already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Email" && e.ErrorMessage.Contains("is already in use"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Url" && e.ErrorMessage.Contains("is already in use"));

            Assert.NotEqual(institution.Nickname, dtUpdate.Nickname);
            Assert.NotEqual(institution.Email, dtUpdate.Email);
            Assert.False(PasswordHasher.Verify(dtUpdate.Password, institution.Password));
            Assert.NotEqual(institution.Description, dtUpdate.Description);
            Assert.NotEqual(institution.Url, dtUpdate.Url);
        }

        [Fact]
        public async Task Update_When_Institution_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();

            var institution = new Institution(
                    "nickname 1",
                    "Password1234",
                    "Acronym 1",
                    "email1@gmail.com",
                    "name 1",
                    "description 1",
                    "https://urlUno.com",
                    "country 1",
                    "city 1",
                    "street 1",
                    "0000",
                    1
                    );

            var dtUpdate = new DTUpdateInstitution(
                    Guid.NewGuid(),
                    "Updated",
                    "Updated01234",
                    "updated@gmail.com",
                    "Updated",
                    "https://Updated.com"
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(dtUpdate.Id)).ReturnsAsync(institution);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var (result, validation) = await service.UpdateAsync(dtUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTInstitution>(result);

            // DT created well
            Assert.Equal(institution.Id, result.Id);
            Assert.Equal(institution.Nickname, result.Nickname);
            Assert.Equal(institution.Email, result.Email);
            Assert.Equal(institution.Name, result.Name);
            Assert.Equal(institution.Acronym, result.Acronym);
            Assert.Equal(institution.Description, result.Description);
            Assert.Equal(institution.Url, result.Url);
            Assert.Equal(institution.Country, result.Country);
            Assert.Equal(institution.City, result.City);
            Assert.Equal(institution.Street, result.Street);
            Assert.Equal(institution.Number, result.Number);
            Assert.Equal(institution.Floor, result.Floor);
            Assert.Equal(institution.Created, result.Created);

            // Client Updated
            Assert.Equal(institution.Nickname, dtUpdate.Nickname);
            Assert.True(PasswordHasher.Verify(dtUpdate.Password, institution.Password));
            Assert.Equal(institution.Email, dtUpdate.Email);
            Assert.Equal(institution.Url, dtUpdate.Url);
            Assert.Equal(institution.Description, dtUpdate.Description);
        }

        [Fact]
        public async Task Delete_When_Institution_Does_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();

            var id = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Institution?)null);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_When_Institution_Does_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IInstitutionRepo>();

            var institution = new Institution(
                    "nickname 1",
                    "Password1234",
                    "Acronym 1",
                    "email1@gmail.com",
                    "name 1",
                    "description 1",
                    "https://urlUno.com",
                    "country 1",
                    "city 1",
                    "street 1",
                    "0000",
                    1
                    );

            mockRepo.Setup(repo => repo.GetByIdAsync(institution.Id)).ReturnsAsync(institution);

            var service = new InstitutionService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(institution.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DTInstitution>(result);

            // DT created well
            Assert.Equal(institution.Id, result.Id);
            Assert.Equal(institution.Nickname, result.Nickname);
            Assert.Equal(institution.Email, result.Email);
            Assert.Equal(institution.Name, result.Name);
            Assert.Equal(institution.Acronym, result.Acronym);
            Assert.Equal(institution.Description, result.Description);
            Assert.Equal(institution.Url, result.Url);
            Assert.Equal(institution.Country, result.Country);
            Assert.Equal(institution.City, result.City);
            Assert.Equal(institution.Street, result.Street);
            Assert.Equal(institution.Number, result.Number);
            Assert.Equal(institution.Floor, result.Floor);
            Assert.Equal(institution.Created, result.Created);

            // Client Updated
            Assert.False(institution.Active);
        }
    }
}
