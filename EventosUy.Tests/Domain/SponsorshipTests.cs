using EventosUy.Domain.Entities;
using EventosUy.Domain.Enumerates;
using EventosUy.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventosUy.Tests.Domain
{
    public class SponsorshipTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess() 
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var institutionAddress = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, institutionAddress, "description").Value!;
            var eventId = Guid.NewGuid();

            var editionAddress = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, editionAddress, eventId, institutionInstance.Id).Value!;

            var registerTypeIntsance = RegisterType.Create("name", "description", 0, 1, editionInstance.Id).Value!;

            var sponsorLevelInstance = SponsorLevel.Create(1_000, SponsorshipTier.BRONZE).Value!;

            // Act
            var result = Sponsorship.Create("name", 1, "code", sponsorLevelInstance, institutionInstance, editionInstance, registerTypeIntsance, new DateOnly(3000, 2, 2));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("name", result.Value.Name);
            Assert.Equal(1, result.Value.Free);
            Assert.Equal("code", result.Value.Code);
            Assert.Equal(sponsorLevelInstance, result.Value.Level);
            Assert.Equal(institutionInstance.Id, result.Value.Institution);
            Assert.Equal(editionInstance.Id, result.Value.Edition);
            Assert.Equal(registerTypeIntsance.Id, result.Value.RegisterType);
        }

        public static IEnumerable<object?[]> InvalidInput()
        {
            yield return new object?[]
            {
                "", -1, "", null, null, null,  null, new DateOnly(1990, 2, 2),
                new List<string>
                {
                    "Sponsor level cannot be empty.",
                    "Institution cannot be empty.",
                    "Edition cannot be empty.",
                    "Register type cannot be empty.",
                    "Name cannot be empty.",
                    "Code cannot be empty.",
                    "Amount must be greater than or equal to 0.",
                    "Expiration date must be after today's date."
                }
             };

            
            var levelInstance = SponsorLevel.Create(10_000m, SponsorshipTier.SILVER).Value;
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var addressInst = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, addressInst, "description").Value!;

            var eventId = Guid.NewGuid();
            var addressEdition = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, addressEdition, eventId, institutionInstance.Id).Value!;

            var registerTypeInstance = RegisterType.Create("name", "description", 500m, 100, editionInstance.Id).Value!;

            yield return new object?[]
            {
                "name", 5, "code", levelInstance, institutionInstance, editionInstance, registerTypeInstance, new DateOnly(4000, 12, 8),
                new List<string> { "The value of free registrations may not exceed 20% of the institution's contribution." }
            };

            yield return new object?[]
            {
                " ", 0, " ",  levelInstance, institutionInstance, editionInstance, registerTypeInstance, new DateOnly(4000, 12, 8),
                new List<string>
                {
                    "Name cannot be empty.",
                    "Code cannot be empty."
                }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidInput))]
        public void Create_WithInvalidInput_ReturnsSuccess(
            string name,
            int free,
            string code,
            SponsorLevel? level,
            Institution? institution,
            Edition? edition,
            RegisterType? registerType,
            DateOnly expired,
            List<string> expectedErrors
            )
        {
            // Act
            var result = Sponsorship.Create(name, free, code, level, institution, edition, registerType, expired);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            foreach (var expectedError in expectedErrors)
            {
                Assert.Contains(expectedError, result.Errors);
            }
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var institutionAddress = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, institutionAddress, "description").Value!;
            var eventId = Guid.NewGuid();

            var editionAddress = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, editionAddress, eventId, institutionInstance.Id).Value!;

            var registerTypeIntsance = RegisterType.Create("name", "description", 0, 1, editionInstance.Id).Value!;

            var sponsorLevelInstance = SponsorLevel.Create(1_000, SponsorshipTier.BRONZE).Value!;

            var sponsorInstance = Sponsorship.Create("name", 1, "code", sponsorLevelInstance, institutionInstance, editionInstance, registerTypeIntsance, new DateOnly(3000, 2, 2)).Value!;

            // Act
            var dtSponsor = sponsorInstance.GetDT(editionInstance, institutionInstance);

            // Assert
            Assert.NotNull(dtSponsor);
            Assert.Equal(sponsorInstance.Name, dtSponsor.Name);
            Assert.Equal(sponsorInstance.Expired, dtSponsor.Expired);
            Assert.Equal(sponsorInstance.Level.Amount, dtSponsor.Amount);
            Assert.Equal(sponsorInstance.Level.Tier, dtSponsor.Tier);
            Assert.Equal(sponsorInstance.Free, dtSponsor.Free);
            Assert.Equal(sponsorInstance.Code, dtSponsor.Code);
            Assert.Equal(institutionInstance.Nickname, dtSponsor.Institution);
            Assert.Equal(editionInstance.Name, dtSponsor.Edition);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var institutionAddress = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, institutionAddress, "description").Value!;
            var eventId = Guid.NewGuid();

            var editionAddress = Address.Create("country", "city", "street", "number").Value!;
            var from = new DateOnly(3000, 8, 8);
            var to = new DateOnly(3000, 12, 8);
            var editionInstance = Edition.Create("name", "initials", from, to, editionAddress, eventId, institutionInstance.Id).Value!;

            var registerTypeIntsance = RegisterType.Create("name", "description", 0, 1, editionInstance.Id).Value!;

            var sponsorLevelInstance = SponsorLevel.Create(1_000, SponsorshipTier.BRONZE).Value!;

            var sponsorInstance = Sponsorship.Create("name", 1, "code", sponsorLevelInstance, institutionInstance, editionInstance, registerTypeIntsance, new DateOnly(3000, 2, 2)).Value!;

            // Act
            var card = sponsorInstance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(sponsorInstance.Id, card.Id);
            Assert.Equal(sponsorInstance.Name, card.Name);
            Assert.Equal(sponsorInstance.Expired, card.expiration);
            Assert.Equal(sponsorInstance.Level.Tier, card.Tier);
        }
    }
}
