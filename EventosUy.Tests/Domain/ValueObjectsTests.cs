using EventosUy.Domain.ValueObjects;

namespace EventosUy.Tests.Domain
{
    public class ValueObjectsTests
    {
        [Fact]
        public void Password_Create_WithValidInput_ReturnsSuccess() 
        {
            // Act
            var result = Password.Create("Password1234");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.True(result.Value.Verify("Password1234"));
        }

        [Theory]
        [InlineData("", "Password cannot be empty.")]
        [InlineData(" ", "Password cannot be empty.")]
        [InlineData("short", "Password must have at least 12 characters.")]
        [InlineData("ABCDEFGHIJKL", "Password must contain lower and upper cases.")]
        [InlineData("abcdefghijkl", "Password must contain lower and upper cases.")]
        [InlineData("Passwordabcd", "Password must contain digits.")]
        [InlineData("Password 1234", "Password cannot contain whitespace.")]
        public void Password_Create_WithInvalidInput_ReturnsFailure(string value, string expected) 
        {
            // Act
            var result = Password.Create(value);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(expected, result.Errors[0]);
        }

        [Theory]
        [InlineData("https://inst.com")]
        [InlineData("http://inst.com")]
        public void Url_Create_WithValidInput_ReturnsSuccess(string rawUrl)
        {
            // Act
            var result = Url.Create(rawUrl);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(rawUrl, result.Value.Value);
        }

        [Theory]
        [InlineData("", "URL can not be empty.")]
        [InlineData(" ", "URL can not be empty.")]
        [InlineData("google.com", "URL is not formatted correctly.")]
        [InlineData("http", "URL is not formatted correctly.")]
        public void Url_Create_WithInvalidInput_ReturnsFailure(string value, string expected)
        {
            // Act
            var result = Url.Create(value);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(expected, result.Errors[0]);
        }

        [Fact]
        public void Email_Create_WithValidInput_ReturnsSuccess()
        {
            // Act
            var result = Email.Create("prueba@gmail.com");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("prueba@gmail.com", result.Value.Value);
        }

        [Theory]
        [InlineData("", "Email cannot be empty.")]
        [InlineData(" ", "Email cannot be empty.")]
        [InlineData("short", "Email is not formatted correctly.")]
        [InlineData("@", "Email is not formatted correctly.")]
        [InlineData("a@a", "Email is not formatted correctly.")]
        [InlineData("@.", "Email is not formatted correctly.")]
        [InlineData("pr ueba@ .com", "Email is not formatted correctly.")]
        public void Email_Create_WithInvalidInput_ReturnsFailure(string value, string expected)
        {
            // Act
            var result = Email.Create(value);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(expected, result.Errors[0]);
        }

        [Theory]
        [InlineData("firstSurname", "lastSurname", "firstName", "lastName")]
        [InlineData("firstSurname", "lastSurname", "firstName", null)]
        public void Name_Create_WithValidInput_ReturnsSuccess(string firstSurname, string lastSurname, string firstName, string lastName)
        {
            // Act
            var result = Name.Create(firstSurname, lastSurname, firstName, lastName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(firstSurname, result.Value.FirstSurname);
            Assert.Equal(lastSurname, result.Value.LastSurname);
            Assert.Equal(firstName, result.Value.FirstName);
            Assert.Equal(lastName, result.Value.LastName);
        }

        public static IEnumerable<object[]> InvalidInputs()
        {
            yield return new object[] { "", "", "", null, 
                new[] { 
                    "First surname cannot be empty.", 
                    "Last surname cannot be empty.", 
                    "First name cannot be empty." 
                } 
            };
            yield return new object[] { "1Surname", "LSur/name", "FN ame", " ", 
                new[] { 
                    "First surname cannot have anythings else than letters.", 
                    "Last surname cannot have anythings else than letters.",
                    "First name cannot have anythings else than letters.",
                    "Last name cannot have anythings else than letters."
                } 
            };
            yield return new object[] { "fs", "ls", "fs", "++",
                new[] { "Last name cannot have anythings else than letters." }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidInputs))]
        public void Name_Create_WithInvalidInput_ReturnsFailure(string firstSurname, string lastSurname, string firstName, string? lastName, string[] expected)
        {
            // Act
            var result = Name.Create(firstSurname, lastSurname, firstName, lastName);

            // Assert
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(expected.Length, result.Errors.Count);
            foreach (var expectedError in expected) 
            {
                Assert.Contains(expectedError, result.Errors);
            }
        }
    }
}
