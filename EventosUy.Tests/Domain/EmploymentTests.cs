using EventosUy.Domain.Entities;
using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class EmploymentTests
    {
        [Fact]
        public void Create_WithValidInput_ReturnsSuccess() 
        {
            // Arrange
            var from = new DateOnly(3000, 5, 6);
            var to = new DateOnly(3000, 10, 6);
            var jobTitleId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var institutionId = Guid.NewGuid();

            // Act
            var result = Employment.Create(from, to, jobTitleId, professionalId, institutionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(from, result.Value.From);
            Assert.Equal(to, result.Value.To);
            Assert.Equal(jobTitleId, result.Value.JobTitle);
            Assert.Equal(professionalId, result.Value.ProfessionalProfile);
            Assert.Equal(institutionId, result.Value.Institution);
        }

        [Fact]
        public void Create_WithInvalidInput_ReturnsFailure()
        {
            // Arrange
            var from = new DateOnly(2000, 5, 6);
            var to = new DateOnly(1990, 10, 6);
            var jobTitleId = Guid.Empty;
            var professionalId = Guid.Empty;
            var institutionId = Guid.Empty;

            // Act
            var result = Employment.Create(from, to, jobTitleId, professionalId, institutionId);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(5, result.Errors.Count);
            Assert.Equal("Starting date cannot be after ending's date", result.Errors[0]);
            Assert.Equal("Starting date cannot be before todays's date", result.Errors[1]);
            Assert.Equal("JobTitle cannot me empty.", result.Errors[2]);
            Assert.Equal("Professional profile cannot me empty.", result.Errors[3]);
            Assert.Equal("Institution cannot me empty.", result.Errors[4]);
        }

        [Fact]
        public void GetDT_ReturnsCorrectData()
        {
            // Arrange
            var passwordInstitution = Password.Create("PassWord1234").Value!;
            var emailInstitution = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", passwordInstitution, emailInstitution, "name", url, address, "description").Value!;

            var jobTitleInstance = JobTitle.Create("jobName", "jobDescription", institutionInstance.Id).Value!;

            var passwordPerson = Password.Create("PassWord1234").Value!;
            var emailPerson = Email.Create("person@gmail.com").Value!;
            var name = Name.Create("firstsurname", "lastsurname", "firstname", "lastname").Value!;
            var birthday = new DateOnly(1985, 5, 12);
            var personInstance = Person.Create("nickname", passwordPerson, emailPerson, name, birthday).Value!;

            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };
            var professionalInstance = ProfessionalProfile.Create(personInstance.Id, linkTree, specialities).Value!;

            var from = new DateOnly(3000, 5, 6);
            var to = new DateOnly(3000, 10, 6);
            var employmentInstance = Employment.Create(from, to, jobTitleInstance.Id, professionalInstance.Id, institutionInstance.Id).Value!;

            // Act
            var dtEmployment = employmentInstance.GetDT(institutionInstance, jobTitleInstance);

            // Assert
            Assert.NotNull(dtEmployment);
            Assert.Equal(institutionInstance.Nickname, dtEmployment.Institution);
            Assert.Equal(jobTitleInstance.Name, dtEmployment.JobTitle);
            Assert.Equal(from, dtEmployment.From);
            Assert.Equal(to, dtEmployment.To);
        }

        [Fact]
        public void GetCardByPerson_ReturnsCorrectData()
        {
            // Arrange
            var passwordInstitution = Password.Create("PassWord1234").Value!;
            var emailInstitution = Email.Create("institution@gmail.com").Value!;
            var url = Url.Create("https://inst.com").Value!;
            var address = Address.Create("country", "city", "street", "0124").Value!;
            var institutionInstance = Institution.Create("nickname", passwordInstitution, emailInstitution, "name", url, address, "description").Value!;

            var jobTitleInstance = JobTitle.Create("jobName", "jobDescription", institutionInstance.Id).Value!;

            var personId = Guid.NewGuid();

            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };
            var professionalInstance = ProfessionalProfile.Create(personId, linkTree, specialities).Value!;

            var from = new DateOnly(3000, 5, 6);
            var to = new DateOnly(3000, 10, 6);
            var employmentInstance = Employment.Create(from, to, jobTitleInstance.Id, professionalInstance.Id, institutionInstance.Id).Value!;

            // Act
            var card = employmentInstance.GetCardByPerson(institutionInstance, jobTitleInstance);

            // Assert
            Assert.NotNull(card);
            Assert.Equal(institutionInstance.Nickname, card.Institution);
            Assert.Equal(jobTitleInstance.Name, card.JobTitle);
        }

        [Fact]
        public void GetCardByInstitution_ReturnsCorrectData()
        {
            // Arrange
            var institutionId = Guid.NewGuid();

            var jobTitleInstance = JobTitle.Create("jobName", "jobDescription", institutionId).Value!;

            var passwordPerson = Password.Create("PassWord1234").Value!;
            var emailPerson = Email.Create("person@gmail.com").Value!;
            var name = Name.Create("firstsurname", "lastsurname", "firstname", "lastname").Value!;
            var birthday = new DateOnly(1985, 5, 12);
            var personInstance = Person.Create("nickname", passwordPerson, emailPerson, name, birthday).Value!;

            var linkTree = Url.Create("https://linkTree.com").Value!;
            var specialities = new List<string> { "S1", "S2", "S3" };
            var professionalInstance = ProfessionalProfile.Create(personInstance.Id, linkTree, specialities).Value!;

            var from = new DateOnly(3000, 5, 6);
            var to = new DateOnly(3000, 10, 6);
            var employmentInstance = Employment.Create(from, to, jobTitleInstance.Id, professionalInstance.Id, institutionId).Value!;

            // Act
            var card = employmentInstance.GetCardByInstitution(personInstance, jobTitleInstance);

            // Assert
            Assert.NotNull(card);
            Assert.Equal(personInstance.Nickname, card.Person);
            Assert.Equal(jobTitleInstance.Name, card.JobTitle);
        }
    }
}
