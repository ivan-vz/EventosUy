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
    public class SponsorshipServiceTests
    {
        [Fact]
        public async Task Create_With_Non_Existant_Data() 
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var dtInsert = new DTInsertSponsorship(
                    "name",
                    SponsorshipTier.BRONZE,
                    100, 
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "voucher",
                    "voucherCode"
                );

            mockInstitutionService.Setup(s => s.GetByIdAsync(dtInsert.Institution)).ReturnsAsync((null, null));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((null, null));

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count);

            Assert.Contains(validation.Errors, e => e.PropertyName == "Institution" && e.ErrorMessage.Contains("not found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register Type" && e.ErrorMessage.Contains("not found"));
        }

        [Theory]
        [InlineData(999, "must be at least")]
        [InlineData(10_000, "exceeds maximum for")]
        public async Task Create_With_Invalid_Data(decimal amount, string expected)
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

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

            var dtRegType = new DTRegisterType(
                    Guid.NewGuid(),
                    "name",
                    "description",
                    0,
                    1,
                    0,
                    DateTime.UtcNow,
                    true,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING)
                );

            var cardRT = new RegisterTypeCard(Guid.NewGuid(), "name", 0, 1);

            var dtInsert = new DTInsertSponsorship(
                    "name",
                    SponsorshipTier.BRONZE,
                    amount,
                    cardIns.Id,
                    cardRT.Id,
                    "voucher",
                    "voucherCode"
                );

            mockInstitutionService.Setup(s => s.GetByIdAsync(dtInsert.Institution)).ReturnsAsync((dtIns, cardIns));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));
            mockRepo.Setup(repo => repo.ExistsAsync(dtRegType.Edition.Id, dtInsert.Institution)).ReturnsAsync(true);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count);

            Assert.Contains(validation.Errors, e => e.PropertyName == "Tier" && e.ErrorMessage.Contains(expected));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Edition | Institution" && e.ErrorMessage.Contains("already exists"));
        }

        [Fact]
        public async Task Create_With_Valid_Data()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

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

            var dtRegType = new DTRegisterType(
                    Guid.NewGuid(),
                    "name",
                    "description",
                    0,
                    1,
                    0,
                    DateTime.UtcNow,
                    true,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING)
                );

            var cardRT = new RegisterTypeCard(Guid.NewGuid(), "name", 0, 1);

            var dtInsert = new DTInsertSponsorship(
                    "name",
                    SponsorshipTier.BRONZE,
                    1_000m,
                    cardIns.Id,
                    cardRT.Id,
                    "voucher",
                    "voucherCode"
                );

            mockInstitutionService.Setup(s => s.GetByIdAsync(dtInsert.Institution)).ReturnsAsync((dtIns, cardIns));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));
            mockRepo.Setup(repo => repo.ExistsAsync(dtRegType.Edition.Id, dtInsert.Institution)).ReturnsAsync(false);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTSponsorship>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Amount, result.Amount);
            Assert.Equal(dtInsert.Tier, result.Tier);
            Assert.Equal(dtRegType.Edition, result.Edition);
            Assert.Equal(cardRT, result.RegisterType);
        }

        [Fact]
        public async Task Get_All_By_Edition_When_Empty() 
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var nonExistantId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByEditionAsync(nonExistantId)).ReturnsAsync([]);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var result = await service.GetAllByEditionAsync(nonExistantId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_All_By_Edition_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var nonExistantId = Guid.NewGuid();

            var sponsorships = new List<Sponsorship>() {
                new ("name 1", 1_000m, SponsorshipTier.BRONZE, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                new ("name 2", 10_000m, SponsorshipTier.SILVER, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
            };

            mockRepo.Setup(repo => repo.GetAllByEditionAsync(nonExistantId)).ReturnsAsync(sponsorships);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var result = (await service.GetAllByEditionAsync(nonExistantId)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(sponsorships[0].Id, result[0].Id);
            Assert.Equal(sponsorships[0].Name, result[0].Name);
            Assert.Equal(sponsorships[0].Tier, result[0].Tier);

            Assert.Equal(sponsorships[1].Id, result[1].Id);
            Assert.Equal(sponsorships[1].Name, result[1].Name);
            Assert.Equal(sponsorships[1].Tier, result[1].Tier);
        }

        [Fact]
        public async Task Get_All_By_Institution_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var nonExistantId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByInstitutionAsync(nonExistantId)).ReturnsAsync([]);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var result = await service.GetAllByInstitutionAsync(nonExistantId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_All_By_Institution_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var id = Guid.NewGuid();

            var sponsorships = new List<Sponsorship>() {
                new ("name 1", 1_000m, SponsorshipTier.BRONZE, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                new ("name 2", 10_000m, SponsorshipTier.SILVER, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
            };

            mockRepo.Setup(repo => repo.GetAllByInstitutionAsync(id)).ReturnsAsync(sponsorships);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var result = (await service.GetAllByInstitutionAsync(id)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(sponsorships[0].Id, result[0].Id);
            Assert.Equal(sponsorships[0].Name, result[0].Name);
            Assert.Equal(sponsorships[0].Tier, result[0].Tier);

            Assert.Equal(sponsorships[1].Id, result[1].Id);
            Assert.Equal(sponsorships[1].Name, result[1].Name);
            Assert.Equal(sponsorships[1].Tier, result[1].Tier);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existant_Id() 
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var nonExistantId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistantId)).ReturnsAsync((Sponsorship?)null);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(nonExistantId);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Get_By_Id_With_Existant_Id()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

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

            var dtRegType = new DTRegisterType(
                    Guid.NewGuid(),
                    "name",
                    "description",
                    0,
                    1,
                    0,
                    DateTime.UtcNow,
                    true,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING)
                );

            var cardRT = new RegisterTypeCard(Guid.NewGuid(), "name", 0, 1);

            var sponsor = new Sponsorship(
                    "name",
                    1_000m,
                    SponsorshipTier.BRONZE,
                    dtRegType.Edition.Id,
                    cardIns.Id,
                    cardRT.Id
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(sponsor.Id)).ReturnsAsync(sponsor);
            mockInstitutionService.Setup(s => s.GetByIdAsync(sponsor.InstitutionId)).ReturnsAsync((dtIns, cardIns));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(sponsor.RegisterTypeId)).ReturnsAsync((dtRegType, cardRT));

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(sponsor.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTSponsorship>(dtResult);
            Assert.IsType<SponsorshipCard>(cardResult);

            Assert.Equal(sponsor.Id, dtResult.Id);
            Assert.Equal(sponsor.Name, dtResult.Name);
            Assert.Equal(sponsor.Amount, dtResult.Amount);
            Assert.Equal(sponsor.Tier, dtResult.Tier);
            Assert.Equal(sponsor.Created, dtResult.Created);
            Assert.Equal(dtRegType.Edition, dtResult.Edition);
            Assert.Equal(cardIns, dtResult.Institution);
            Assert.Equal(cardRT, dtResult.RegisterType);

            Assert.Equal(sponsor.Id, cardResult.Id);
            Assert.Equal(sponsor.Name, cardResult.Name);
            Assert.Equal(sponsor.Tier, cardResult.Tier);
        }

        [Fact]
        public async Task Delete_With_Non_Existant_Id()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

            var nonExistantId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistantId)).ReturnsAsync((Sponsorship?)null);

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var result = await service.DeleteAsync(nonExistantId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_With_Existant_Id()
        {
            // Arrange
            var mockRepo = new Mock<ISponsorshipRepo>();
            var mockInstitutionService = new Mock<IInstitutionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();

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

            var dtRegType = new DTRegisterType(
                    Guid.NewGuid(),
                    "name",
                    "description",
                    0,
                    1,
                    0,
                    DateTime.UtcNow,
                    true,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING)
                );

            var cardRT = new RegisterTypeCard(Guid.NewGuid(), "name", 0, 1);

            var sponsor = new Sponsorship(
                    "name",
                    1_000m,
                    SponsorshipTier.BRONZE,
                    dtRegType.Edition.Id,
                    cardIns.Id,
                    cardRT.Id
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(sponsor.Id)).ReturnsAsync(sponsor);
            mockInstitutionService.Setup(s => s.GetByIdAsync(sponsor.InstitutionId)).ReturnsAsync((dtIns, cardIns));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(sponsor.RegisterTypeId)).ReturnsAsync((dtRegType, cardRT));

            var service = new SponsorshipService(mockRepo.Object, mockInstitutionService.Object, mockRegisterTypeService.Object);

            // Act
            var result = await service.DeleteAsync(sponsor.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DTSponsorship>(result);

            Assert.Equal(sponsor.Id, result.Id);
            Assert.Equal(sponsor.Name, result.Name);
            Assert.Equal(sponsor.Amount, result.Amount);
            Assert.Equal(sponsor.Tier, result.Tier);
            Assert.Equal(sponsor.Created, result.Created);
            Assert.Equal(dtRegType.Edition, result.Edition);
            Assert.Equal(cardIns, result.Institution);
            Assert.Equal(cardRT, result.RegisterType);

            Assert.False(sponsor.Active);
        }
    }
}
