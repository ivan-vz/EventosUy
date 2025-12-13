using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class ProfessionalProfileTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };

            // Act
            var result = ProfessionalProfile.Create(personId, linkTree, specialities);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(personId, result.Value.Id);
            Assert.Equal(linkTree, result.Value.LinkTree);
            foreach (var s in specialities)
            {
                Assert.Contains(s, result.Value.Specialities);
            }
        }

        [Fact]
        public void Create_WithInvalidInput_ReturnsFailure()
        {
            // Arrange
            var personId = Guid.Empty;
            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };

            // Act
            var result = ProfessionalProfile.Create(personId, linkTree, specialities);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Person can not be empty.", result.Errors[0]);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {

            // Arrange
            var personId = Guid.NewGuid();
            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };

            // Act
            var professionalInstance = ProfessionalProfile.Create(personId, linkTree, specialities).Value!;

            // Act
            var dtProfessional = professionalInstance.GetDT();

            // Assert
            Assert.NotNull(dtProfessional);
            Assert.Equal(professionalInstance.LinkTree, dtProfessional.LinkTree);
            Assert.Equal(professionalInstance.Request, dtProfessional.Request);
            Assert.Equal(professionalInstance.Specialities, dtProfessional.Specialities);
            Assert.Equal(professionalInstance.State, dtProfessional.State);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("person@gmail.com").Value!;
            var name = Name.Create("firstsurname", "lastsurname", "firstname", "lastname").Value!;
            var birthday = new DateOnly(1985, 5, 12);
            var personInstance = Person.Create("nickname", password, email, name, birthday).Value!;

            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };
            var professionalInstance = ProfessionalProfile.Create(personInstance.Id, linkTree, specialities).Value!;

            // Act
            var card = professionalInstance.GetCard(personInstance);

            // Assert
            Assert.NotNull(card);
            Assert.Equal(professionalInstance.Id, card.Id);
            Assert.Equal(personInstance.Nickname, card.Nickname);
            Assert.Equal(personInstance.Email.Value, card.Email);
        }
    }
}
