using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class JobTitleTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess() 
        {
            // Arrange
            var institutionId = Guid.NewGuid();

            // Act
            var result = JobTitle.Create("jobName", "jobDescription", institutionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("jobName", result.Value.Name);
            Assert.Equal("jobDescription", result.Value.Description);
            Assert.Equal(institutionId, result.Value.Institution);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void Create_WithInvalidInput_ReturnsFailure(string name, string description) 
        {
            // Arrange
            var institutionId = Guid.Empty;

            // Act
            var result = JobTitle.Create(name, description, institutionId);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(3, result.Errors.Count);
            Assert.Equal("Name can not be empty.", result.Errors[0]);
            Assert.Equal("Description can not be empty.", result.Errors[1]);
            Assert.Equal("Institution can not be empty.", result.Errors[2]);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, address, "description").Value!;

            var jobTitleInstance = JobTitle.Create("jobName", "jobDescription", institutionInstance.Id).Value!;

            // Act
            var dtJobTitle = jobTitleInstance.GetDT(institutionInstance);

            // Assert
            Assert.NotNull(dtJobTitle);
            Assert.Equal(institutionInstance.Nickname, dtJobTitle.Institution);
            Assert.Equal(jobTitleInstance.Name, dtJobTitle.Name);
            Assert.Equal(jobTitleInstance.Description, dtJobTitle.Description);
        }

        [Fact]
        public void GetCard_ReturnsCorrectData()
        {
            // Arrange
            var password = Password.Create("PassWord1234").Value!;
            var email = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", password, email, "name", url, address, "description").Value!;

            var jobTitleInstance = JobTitle.Create("jobName", "jobDescription", institutionInstance.Id).Value!;

            // Act
            var card = jobTitleInstance.GetCard();

            // Assert
            Assert.NotNull(card);
            Assert.Equal(jobTitleInstance.Id, card.Id);
            Assert.Equal(jobTitleInstance.Name, card.Name);
        }
    }
}
