using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class PersonTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("person@gmail.com").Value!;
            var name = Name.Create("firstsurname", "lastsurname", "firstname", "lastname").Value!;
            var birthday = new DateOnly(1985, 5, 12);

            // Act
            var result = Person.Create("nickname", password, email, name, birthday);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("nickname", result.Value.Nickname);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(password, result.Value.Password);
            Assert.Equal(email, result.Value.Email);
            Assert.Equal(birthday, result.Value.Birthday);
        }

        public static IEnumerable<object[]> InvalidInputs() 
        {
            yield return new object[] { "", DateOnly.FromDateTime(DateTime.UtcNow) };
            yield return new object[] { " ", new DateOnly(3000, 4, 23) };
        }

        [Theory]
        [MemberData(nameof(InvalidInputs))]
        public void Create_WithInvalidInput_ReturnsFailure(string nickname, DateOnly birthday)
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("person@gmail.com").Value!;
            var name = Name.Create("firstsurname", "lastsurname", "firstname", "lastname").Value!;

            // Act
            var result = Person.Create(nickname, password, email, name, birthday);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(2, result.Errors.Count);
            Assert.Equal("Nickname can not be empty.", result.Errors[0]);
            Assert.Equal("Invalid Birthday's date.", result.Errors[1]);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("person@gmail.com").Value!;
            var name = Name.Create("firstsurname", "lastsurname", "firstname", "lastname").Value!;
            var birthday = new DateOnly(1985, 5, 12);
            var personInstance = Person.Create("nickname", password, email, name, birthday).Value!;

            // Act
            var dtPerson = personInstance.GetDT();

            // Assert
            Assert.NotNull(dtPerson);
            Assert.Equal(personInstance.Nickname, dtPerson.Nickname);
            Assert.Equal(personInstance.Email.Value, dtPerson.Email);
            Assert.Equal(personInstance.Name.FullName, dtPerson.FullName);
            Assert.Equal(personInstance.Birthday, dtPerson.Birthday);
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

            // Act
            var card = personInstance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(personInstance.Id, card.Id);
            Assert.Equal(personInstance.Nickname, card.Nickname);
            Assert.Equal(personInstance.Email.Value, card.Email);
        }
    }
}
