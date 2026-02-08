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
    public class RegisterServiceTests
    {
        [Fact]
        public async Task Create_With_Voucher_And_Existant_Data() 
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var dtClient = new DTClient(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "firstName",
                    "lastName",
                    "firstSurname",
                    "lastSurname",
                    DateOnly.Parse("1990-10-10"),
                    DateTimeOffset.UtcNow,
                    "0123456"
                );

            var cardClient = new UserCard(dtClient.Id, dtClient.Nickname, dtClient.Email);

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

            var cardRT = new RegisterTypeCard(dtRegType.Id, dtRegType.Name, dtRegType.Price, dtRegType.Quota);

            var dtVoucher = new DTVoucher(
                    Guid.NewGuid(),
                    "name",
                    20, 
                    1,
                    0,
                    DateTime.UtcNow,
                    VoucherState.AVAILABLE,
                    dtRegType.Edition,
                    cardRT,
                    null
                );

            var cardVoucher = new VoucherCard(dtVoucher.Id, dtVoucher.Name);

            var dtInsert = new DTInsertRegisterWithVoucher(cardClient.Id, cardRT.Id, "code");

            decimal discount = dtRegType.Price * dtVoucher.Discount / 100;
            decimal price = dtRegType.Price - discount;

            mockClientService.Setup(s => s.GetByIdAsync(dtInsert.Client)).ReturnsAsync((dtClient, cardClient));
            mockRegisterTypeService.Setup(S => S.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));
            mockVoucherService.Setup(S => S.GetByCodeAsync(dtInsert.Code)).ReturnsAsync((dtVoucher, cardVoucher));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTRegister>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(price, result.Total);
            Assert.Equal(cardRT, result.RegisterType);
            Assert.Equal(dtRegType.Edition, result.Edition);
            Assert.Equal(cardClient, result.Client);
            Assert.Equal(cardVoucher, result.Voucher);
        }

        [Fact]
        public async Task Create_With_Voucher_And_Non_Existant_Data()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var dtInsert = new DTInsertRegisterWithVoucher(Guid.NewGuid(), Guid.NewGuid(), "code");

            mockClientService.Setup(s => s.GetByIdAsync(dtInsert.Client)).ReturnsAsync((null, null));
            mockRegisterTypeService.Setup(S => S.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((null, null));
            mockVoucherService.Setup(S => S.GetByCodeAsync(dtInsert.Code)).ReturnsAsync((null, null));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(3, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Client" && e.ErrorMessage.Contains("not found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register type" && e.ErrorMessage.Contains("not found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Voucher" && e.ErrorMessage.Contains("not found"));
        }

        [Fact]
        public async Task Create_With_Voucher_And_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var dtClient = new DTClient(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "firstName",
                    "lastName",
                    "firstSurname",
                    "lastSurname",
                    DateOnly.Parse("1990-10-10"),
                    DateTimeOffset.UtcNow,
                    "0123456"
                );

            var cardClient = new UserCard(dtClient.Id, dtClient.Nickname, dtClient.Email);

            var dtRegType = new DTRegisterType(
                    Guid.NewGuid(),
                    "name",
                    "description",
                    0,
                    1,
                    1,
                    DateTime.UtcNow,
                    false,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ENDED)
                );

            var cardRT = new RegisterTypeCard(dtRegType.Id, dtRegType.Name, dtRegType.Price, dtRegType.Quota);

            var dtVoucher = new DTVoucher(
                    Guid.NewGuid(),
                    "name",
                    20,
                    1,
                    1,
                    DateTime.UtcNow,
                    VoucherState.AVAILABLE,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ONGOING),
                    new RegisterTypeCard(Guid.NewGuid(), dtRegType.Name, dtRegType.Price, dtRegType.Quota),
                    null
                );

            var cardVoucher = new VoucherCard(dtVoucher.Id, dtVoucher.Name);

            var dtInsert = new DTInsertRegisterWithVoucher(cardClient.Id, cardRT.Id, "code");

            decimal discount = dtRegType.Price * dtVoucher.Discount / 100;
            decimal price = dtRegType.Price - discount;

            mockClientService.Setup(s => s.GetByIdAsync(dtInsert.Client)).ReturnsAsync((dtClient, cardClient));
            mockRegisterTypeService.Setup(S => S.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));
            mockVoucherService.Setup(S => S.GetByCodeAsync(dtInsert.Code)).ReturnsAsync((dtVoucher, cardVoucher));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(5, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Voucher" && e.ErrorMessage.Contains("is not valid"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Edition state" && e.ErrorMessage.Contains("is not available"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register type state" && e.ErrorMessage.Contains("is not available"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Voucher Quota" && e.ErrorMessage.Contains("is already completed"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register Type Quota" && e.ErrorMessage.Contains("is already completed"));
        }

        [Fact]
        public async Task Create_Without_Voucher_And_Existant_Data()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var dtClient = new DTClient(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "firstName",
                    "lastName",
                    "firstSurname",
                    "lastSurname",
                    DateOnly.Parse("1990-10-10"),
                    DateTimeOffset.UtcNow,
                    "0123456"
                );

            var cardClient = new UserCard(dtClient.Id, dtClient.Nickname, dtClient.Email);

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

            var cardRT = new RegisterTypeCard(dtRegType.Id, dtRegType.Name, dtRegType.Price, dtRegType.Quota);

            var dtInsert = new DTInsertRegisterWithoutVoucher(cardClient.Id, cardRT.Id);

            mockClientService.Setup(s => s.GetByIdAsync(dtInsert.Client)).ReturnsAsync((dtClient, cardClient));
            mockRegisterTypeService.Setup(S => S.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.NotNull(result);
            Assert.True(validation.IsValid);
            Assert.IsType<DTRegister>(result);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(dtRegType.Price, result.Total);
            Assert.Equal(cardRT, result.RegisterType);
            Assert.Equal(dtRegType.Edition, result.Edition);
            Assert.Equal(cardClient, result.Client);
            Assert.Null(result.Voucher);
        }

        [Fact]
        public async Task Create_Without_Voucher_And_Non_Existant_Data()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var dtInsert = new DTInsertRegisterWithoutVoucher(Guid.NewGuid(), Guid.NewGuid());

            mockClientService.Setup(s => s.GetByIdAsync(dtInsert.Client)).ReturnsAsync((null, null));
            mockRegisterTypeService.Setup(S => S.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((null, null));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(2, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Client" && e.ErrorMessage.Contains("not found"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register type" && e.ErrorMessage.Contains("not found"));
        }

        [Fact]
        public async Task Create_Without_Voucher_And_Invalid_Data()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var dtClient = new DTClient(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "firstName",
                    "lastName",
                    "firstSurname",
                    "lastSurname",
                    DateOnly.Parse("1990-10-10"),
                    DateTimeOffset.UtcNow,
                    "0123456"
                );

            var cardClient = new UserCard(dtClient.Id, dtClient.Nickname, dtClient.Email);

            var dtRegType = new DTRegisterType(
                    Guid.NewGuid(),
                    "name",
                    "description",
                    0,
                    1,
                    1,
                    DateTime.UtcNow,
                    false,
                    new EditionCard(Guid.NewGuid(), "name", "initials", EditionState.ENDED)
                );

            var cardRT = new RegisterTypeCard(dtRegType.Id, dtRegType.Name, dtRegType.Price, dtRegType.Quota);

            var dtInsert = new DTInsertRegisterWithoutVoucher(cardClient.Id, cardRT.Id);

            mockClientService.Setup(s => s.GetByIdAsync(dtInsert.Client)).ReturnsAsync((dtClient, cardClient));
            mockRegisterTypeService.Setup(S => S.GetByIdAsync(dtInsert.RegisterType)).ReturnsAsync((dtRegType, cardRT));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var (result, validation) = await service.CreateAsync(dtInsert);

            // Assert
            Assert.Null(result);
            Assert.False(validation.IsValid);
            Assert.Equal(3, validation.Errors.Count);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Edition state" && e.ErrorMessage.Contains("is not available"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register type state" && e.ErrorMessage.Contains("is not available"));
            Assert.Contains(validation.Errors, e => e.PropertyName == "Register Type Quota" && e.ErrorMessage.Contains("is already completed"));
        }

        [Fact]
        public async Task Get_All_By_Edition_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var editionId = Guid.NewGuid();

            var dtClient1 = new DTClient(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "firstName",
                    "lastName",
                    "firstSurname",
                    "lastSurname",
                    DateOnly.Parse("1990-10-10"),
                    DateTimeOffset.UtcNow,
                    "0123456"
                );

            var cardClient1 = new UserCard(dtClient1.Id, dtClient1.Nickname, dtClient1.Email);

            var dtClient2 = new DTClient(
                    Guid.NewGuid(),
                    "nickname",
                    "email@gmail.com",
                    "firstName",
                    "lastName",
                    "firstSurname",
                    "lastSurname",
                    DateOnly.Parse("1990-10-10"),
                    DateTimeOffset.UtcNow,
                    "0123456"
                );

            var cardClient2 = new UserCard(dtClient2.Id, dtClient2.Nickname, dtClient2.Email);

            var registers = new List<Register>() {
                new (10, cardClient1.Id, editionId, Guid.NewGuid(), null),
                new (0, cardClient2.Id, editionId, Guid.NewGuid(), Guid.NewGuid()),
            };

            mockRepo.Setup(repo => repo.GetAllByEditionAsync(editionId)).ReturnsAsync(registers);
            mockClientService.Setup(s => s.GetByIdAsync(cardClient1.Id)).ReturnsAsync((dtClient1, cardClient1));
            mockClientService.Setup(s => s.GetByIdAsync(cardClient2.Id)).ReturnsAsync((dtClient2, cardClient2));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var result = (await service.GetAllByEditionAsync(editionId)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(registers[0].Id, result[0].Id);
            Assert.Equal(cardClient1.Nickname, result[0].Client);
            Assert.Equal(registers[0].Created, result[0].Created);

            Assert.Equal(registers[1].Id, result[1].Id);
            Assert.Equal(cardClient2.Nickname, result[1].Client);
            Assert.Equal(registers[1].Created, result[1].Created);
        }

        [Fact]
        public async Task Get_All_By_Edition_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var editionId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByEditionAsync(editionId)).ReturnsAsync([]);

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var result = (await service.GetAllByEditionAsync(editionId)).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_All_By_Client_When_Data_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var clientId = Guid.NewGuid();

            var dtEdition1 = new DTEdition(
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

            var cardEdition1 = new EditionCard(dtEdition1.Id, dtEdition1.Name, dtEdition1.Initials, dtEdition1.State);

            var dtEdition2 = new DTEdition(
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

            var cardEdition2 = new EditionCard(dtEdition2.Id, dtEdition2.Name, dtEdition2.Initials, dtEdition2.State);

            var registers = new List<Register>() {
                new (10, clientId, dtEdition1.Id, Guid.NewGuid(), null),
                new (0, clientId, dtEdition2.Id, Guid.NewGuid(), Guid.NewGuid()),
            };

            mockRepo.Setup(repo => repo.GetAllByClientAsync(clientId)).ReturnsAsync(registers);
            mockEditionService.Setup(s => s.GetByIdAsync(dtEdition1.Id)).ReturnsAsync((dtEdition1, cardEdition1));
            mockEditionService.Setup(s => s.GetByIdAsync(dtEdition2.Id)).ReturnsAsync((dtEdition2, cardEdition2));

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var result = (await service.GetAllByClientAsync(clientId)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(registers[0].Id, result[0].Id);
            Assert.Equal(cardEdition1.Name, result[0].Edition);
            Assert.Equal(registers[0].Created, result[0].Created);

            Assert.Equal(registers[1].Id, result[1].Id);
            Assert.Equal(cardEdition2.Name, result[1].Edition);
            Assert.Equal(registers[1].Created, result[1].Created);
        }

        [Fact]
        public async Task Get_All_By_Client_When_Empty()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepo>();
            var mockClientService = new Mock<IClientService>();
            var mockEditionService = new Mock<IEditionService>();
            var mockRegisterTypeService = new Mock<IRegisterTypeService>();
            var mockVoucherService = new Mock<IVoucherService>();

            var clientId = Guid.NewGuid();

            mockRepo.Setup(repo => repo.GetAllByClientAsync(clientId)).ReturnsAsync([]);

            var service = new RegisterService(mockRepo.Object, mockClientService.Object, mockEditionService.Object, mockRegisterTypeService.Object, mockVoucherService.Object);

            // Act
            var result = (await service.GetAllByClientAsync(clientId)).ToList();

            // Assert
            Assert.Empty(result);
        }

    }
}
