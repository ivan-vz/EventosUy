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
    public class VoucherServiceTests
    {

        [Fact]
        public async Task Create_With_Sponsor_And_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var dtInsert = new DTInsertVoucherWithSponsor(
                    "name",
                    "code",
                    100,
                    false, 
                    Guid.NewGuid()
                );

            mockSponsorshipService.Setup(s => s.GetByIdAsync(dtInsert.Sponsor)).ReturnsAsync((null, null));
            mockRepo.Setup(repo => repo.ExistsAsync(dtInsert.Code)).ReturnsAsync(true);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count);

            Assert.Contains(validation.Errors, e => e.PropertyName == "Sponsorship" && e.ErrorMessage.Contains("not found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Code" && e.ErrorMessage.Contains("already in use"));
        }

        [Fact]
        public async Task Create_Without_Sponsor_And_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var dtInsert = new DTInsertVoucherWithoutSponsor(
                    "name",
                    "code",
                    100,
                    false,
                    Guid.NewGuid()
                );

            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((null, null));
            mockRepo.Setup(repo => repo.ExistsAsync(dtInsert.Code)).ReturnsAsync(true);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count);

            Assert.Contains(validation.Errors, e => e.PropertyName == "Register Type" && e.ErrorMessage.Contains("not found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Code" && e.ErrorMessage.Contains("already in use"));
        }

        [Fact]
        public async Task Create_With_Sponsor_And_Valid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var dtSponsor = new DTSponsorship(
                    Guid.NewGuid(),
                    "name",
                    1_000m,
                    SponsorshipTier.BRONZE,
                    DateTime.UtcNow,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING),
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com"),
                    new RegisterTypeCard(Guid.NewGuid(), "name", 1, 10)
                );

            var cardSponsor = new SponsorshipCard(dtSponsor.Id, dtSponsor.Name, dtSponsor.Tier);

            var expectedQuota = Math.Floor((0.2m * dtSponsor.Amount) / dtSponsor.RegisterType.Price);

            var dtInsert = new DTInsertVoucherWithSponsor(
                    "name",
                    "code",
                    100,
                    false,
                    dtSponsor.Id
                );

            mockSponsorshipService.Setup(s => s.GetByIdAsync(dtInsert.Sponsor)).ReturnsAsync((dtSponsor, cardSponsor));
            mockRepo.Setup(repo => repo.ExistsAsync(dtInsert.Code)).ReturnsAsync(false);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTVoucher>(result);


            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Discount, result.Discount);
            Assert.Equal(expectedQuota, result.Quota);
            Assert.Equal(0, result.Used);
            Assert.Equal(VoucherState.AVAILABLE, result.State);
            Assert.Equal(dtSponsor.Edition, result.Edition);
            Assert.Equal(dtSponsor.RegisterType, result.RegisterType);
            Assert.Equal(cardSponsor, result.Sponsorship);
        }

        [Fact]
        public async Task Create_Without_Sponsor_And_Valid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

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

            var dtInsert = new DTInsertVoucherWithoutSponsor(
                    "name",
                    "code",
                    100,
                    false,
                    dtRegType.Id
                );

            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));
            mockRepo.Setup(repo => repo.ExistsAsync(dtInsert.Code)).ReturnsAsync(false);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTVoucher>(result);


            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtInsert.Name, result.Name);
            Assert.Equal(dtInsert.Discount, result.Discount);
            Assert.Equal(dtRegType.Quota, result.Quota);
            Assert.Equal(0, result.Used);
            Assert.Equal(VoucherState.AVAILABLE, result.State);
            Assert.Equal(dtRegType.Edition, result.Edition);
            Assert.Equal(cardRT, result.RegisterType);
            Assert.Null(result.Sponsorship);
        }

        [Fact]
        public async Task Get_By_Id_With_Non_Existent_Id() 
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var nonExistantId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetByIdAsync(nonExistantId)).ReturnsAsync((Voucher?)null);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(nonExistantId);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Get_By_Id_With_Existent_Data_Without_Sponsor()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

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

            var voucher = new Voucher(
                    "name",
                    "code",
                    100,
                    1,
                    true,
                    dtRegType.Edition.Id,
                    dtRegType.Id,
                    null
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(voucher.Id)).ReturnsAsync(voucher);
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtRegType.Id)).ReturnsAsync((dtRegType, cardRT));

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(voucher.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTVoucher>(dtResult);
            Assert.IsType<VoucherCard>(cardResult);

            Assert.Equal(voucher.Id, dtResult.Id);
            Assert.Equal(voucher.Name, dtResult.Name);
            Assert.Equal(voucher.Discount, dtResult.Discount);
            Assert.Equal(voucher.Quota, dtResult.Quota);
            Assert.Equal(voucher.Used, dtResult.Used);
            Assert.Equal(voucher.Created, dtResult.Created);
            Assert.Equal(voucher.State, dtResult.State);
            Assert.Equal(dtRegType.Edition, dtResult.Edition);
            Assert.Equal(cardRT, dtResult.RegisterType);
            Assert.Null(dtResult.Sponsorship);

            Assert.Equal(voucher.Id, cardResult.Id);
            Assert.Equal(voucher.Name, cardResult.Name);
        }

        [Fact]
        public async Task Get_By_Id_With_Existent_Data_With_Sponsor()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

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

            var dtSponsor = new DTSponsorship(
                    Guid.NewGuid(),
                    "name",
                    1_000m, 
                    SponsorshipTier.BRONZE,
                    DateTimeOffset.UtcNow,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING),
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com"),
                    cardRT
                );

            var sponsorCard = new SponsorshipCard(dtSponsor.Id, dtSponsor.Name, dtSponsor.Tier);

            var voucher = new Voucher(
                    "name",
                    "code",
                    100,
                    1,
                    true,
                    dtRegType.Edition.Id,
                    dtRegType.Id,
                    dtSponsor.Id
                );

            mockRepo.Setup(repo => repo.GetByIdAsync(voucher.Id)).ReturnsAsync(voucher);
            mockSponsorshipService.Setup(s => s.GetByIdAsync(dtSponsor.Id)).ReturnsAsync((dtSponsor, sponsorCard));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtRegType.Id)).ReturnsAsync((dtRegType, cardRT));

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByIdAsync(voucher.Id);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTVoucher>(dtResult);
            Assert.IsType<VoucherCard>(cardResult);

            Assert.Equal(voucher.Id, dtResult.Id);
            Assert.Equal(voucher.Name, dtResult.Name);
            Assert.Equal(voucher.Discount, dtResult.Discount);
            Assert.Equal(voucher.Quota, dtResult.Quota);
            Assert.Equal(voucher.Used, dtResult.Used);
            Assert.Equal(voucher.Created, dtResult.Created);
            Assert.Equal(voucher.State, dtResult.State);
            Assert.Equal(dtRegType.Edition, dtResult.Edition);
            Assert.Equal(cardRT, dtResult.RegisterType);
            Assert.Equal(sponsorCard, dtResult.Sponsorship);

            Assert.Equal(voucher.Id, cardResult.Id);
            Assert.Equal(voucher.Name, cardResult.Name);
        }

        [Fact]
        public async Task Get_By_Code_With_Non_Existent_Code()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var nonExistantcode = "code";

            mockRepo.Setup(repo => repo.GetByCodeAsync(nonExistantcode)).ReturnsAsync((Voucher?)null);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByCodeAsync(nonExistantcode);

            // Assert
            Assert.Null(dtResult);
            Assert.Null(cardResult);
        }

        [Fact]
        public async Task Get_By_Code_With_Existent_Data_Without_Sponsor()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

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

            var voucher = new Voucher(
                    "name",
                    "code",
                    100,
                    1,
                    true,
                    dtRegType.Edition.Id,
                    dtRegType.Id,
                    null
                );

            mockRepo.Setup(repo => repo.GetByCodeAsync(voucher.Code)).ReturnsAsync(voucher);
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtRegType.Id)).ReturnsAsync((dtRegType, cardRT));

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByCodeAsync(voucher.Code);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTVoucher>(dtResult);
            Assert.IsType<VoucherCard>(cardResult);

            Assert.Equal(voucher.Id, dtResult.Id);
            Assert.Equal(voucher.Name, dtResult.Name);
            Assert.Equal(voucher.Discount, dtResult.Discount);
            Assert.Equal(voucher.Quota, dtResult.Quota);
            Assert.Equal(voucher.Used, dtResult.Used);
            Assert.Equal(voucher.Created, dtResult.Created);
            Assert.Equal(voucher.State, dtResult.State);
            Assert.Equal(dtRegType.Edition, dtResult.Edition);
            Assert.Equal(cardRT, dtResult.RegisterType);
            Assert.Null(dtResult.Sponsorship);

            Assert.Equal(voucher.Id, cardResult.Id);
            Assert.Equal(voucher.Name, cardResult.Name);
        }

        [Fact]
        public async Task Get_By_Code_With_Existent_Data_With_Sponsor()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

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

            var dtSponsor = new DTSponsorship(
                    Guid.NewGuid(),
                    "name",
                    1_000m,
                    SponsorshipTier.BRONZE,
                    DateTimeOffset.UtcNow,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING),
                    new UserCard(Guid.NewGuid(), "nickname", "email@gmail.com"),
                    cardRT
                );

            var sponsorCard = new SponsorshipCard(dtSponsor.Id, dtSponsor.Name, dtSponsor.Tier);

            var voucher = new Voucher(
                    "name",
                    "code",
                    100,
                    1,
                    true,
                    dtRegType.Edition.Id,
                    dtRegType.Id,
                    dtSponsor.Id
                );

            mockRepo.Setup(repo => repo.GetByCodeAsync(voucher.Code)).ReturnsAsync(voucher);
            mockSponsorshipService.Setup(s => s.GetByIdAsync(dtSponsor.Id)).ReturnsAsync((dtSponsor, sponsorCard));
            mockRegisterTypeService.Setup(s => s.GetByIdAsync(dtRegType.Id)).ReturnsAsync((dtRegType, cardRT));

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            var (dtResult, cardResult) = await service.GetByCodeAsync(voucher.Code);

            // Assert
            Assert.NotNull(dtResult);
            Assert.NotNull(cardResult);
            Assert.IsType<DTVoucher>(dtResult);
            Assert.IsType<VoucherCard>(cardResult);

            Assert.Equal(voucher.Id, dtResult.Id);
            Assert.Equal(voucher.Name, dtResult.Name);
            Assert.Equal(voucher.Discount, dtResult.Discount);
            Assert.Equal(voucher.Quota, dtResult.Quota);
            Assert.Equal(voucher.Used, dtResult.Used);
            Assert.Equal(voucher.Created, dtResult.Created);
            Assert.Equal(voucher.State, dtResult.State);
            Assert.Equal(dtRegType.Edition, dtResult.Edition);
            Assert.Equal(cardRT, dtResult.RegisterType);
            Assert.Equal(sponsorCard, dtResult.Sponsorship);

            Assert.Equal(voucher.Id, cardResult.Id);
            Assert.Equal(voucher.Name, cardResult.Name);
        }

        [Fact]
        public async Task Use_Spot()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var voucher = new Voucher(
                    "name",
                    "code",
                    100,
                    3,
                    true,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                );

            var used = voucher.Used;

            mockRepo.Setup(repo => repo.GetByIdAsync(voucher.Id)).ReturnsAsync(voucher);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            await service.UseSpotAsync(voucher.Id);

            // Assert
            Assert.NotEqual(voucher.Quota, voucher.Used);
            Assert.True((used + 1) == voucher.Used);
            Assert.Equal(VoucherState.AVAILABLE, voucher.State);
        }

        [Fact]
        public async Task Use_All_Spots()
        {
            // Arrange
            var mockRepo = new Mock<IVoucherRepo>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockSponsorshipService = new Mock<ISponsorshipService>();

            var voucher = new Voucher(
                    "name",
                    "code",
                    100,
                    1,
                    true,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                );

            var used = voucher.Used;

            mockRepo.Setup(repo => repo.GetByIdAsync(voucher.Id)).ReturnsAsync(voucher);

            var service = new VoucherService(mockRepo.Object, mockRegisterTypeService.Object, mockSponsorshipService.Object);

            // Act
            await service.UseSpotAsync(voucher.Id);

            // Assert
            Assert.Equal(voucher.Quota, voucher.Used);
            Assert.Equal(VoucherState.COMPLETED, voucher.State);
        }
    }
}
