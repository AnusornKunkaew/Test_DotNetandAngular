using System;
using IT04Solution.Application.Services;
using Xunit;

namespace IT04Solution.Tests
{
    public class ValidationServiceTests
    {
        private readonly ValidationService _validationService;

        public ValidationServiceTests()
        {
            _validationService = new ValidationService();
        }

        #region FirstName Validation Tests
        [Fact]
        public void ValidateEmployee_WithNullFirstName_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = null,
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("First Name is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithEmptyFirstName_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("First Name is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithWhitespaceFirstName_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "   ",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("First Name is required", result.Errors);
        }
        #endregion

        #region LastName Validation Tests
        [Fact]
        public void ValidateEmployee_WithNullLastName_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = null,
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Last Name is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithEmptyLastName_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Last Name is required", result.Errors);
        }
        #endregion

        #region Email Validation Tests
        [Fact]
        public void ValidateEmployee_WithNullEmail_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = null,
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Email is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithEmptyEmail_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Email is required", result.Errors);
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("invalid@")]
        [InlineData("@invalid.com")]
        [InlineData("invalid..@example.com")]
        public void ValidateEmployee_WithInvalidEmailFormat_ReturnsError(string invalidEmail)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = invalidEmail,
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Email format is invalid", result.Errors);
        }

        [Theory]
        [InlineData("john@example.com")]
        [InlineData("john.doe@company.co.th")]
        [InlineData("test+tag@domain.com")]
        public void ValidateEmployee_WithValidEmail_PassesValidation(string validEmail)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = validEmail,
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.DoesNotContain("Email format is invalid", result.Errors);
        }
        #endregion

        #region Phone Validation Tests
        [Fact]
        public void ValidateEmployee_WithNullPhone_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = null,
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Phone is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithEmptyPhone_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Phone is required", result.Errors);
        }

        [Theory]
        [InlineData("081234567")]      // 9 digits - valid
        [InlineData("0812345678")]     // 10 digits - valid
        [InlineData("08-123-4567")]    // 10 digits with formatting
        [InlineData("+66812345678")]   // international format
        public void ValidateEmployee_WithValidPhone_PassesValidation(string validPhone)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = validPhone,
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.DoesNotContain("Phone format is invalid", result.Errors);
        }

        [Theory]
        [InlineData("08123456")]       // 8 digits - too short
        [InlineData("081234567891")]   // 11 digits - too long
        [InlineData("123")]            // too short
        public void ValidateEmployee_WithInvalidPhoneLength_ReturnsError(string invalidPhone)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = invalidPhone,
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Phone format is invalid", result.Errors);
        }
        #endregion

        #region BirthDay Validation Tests
        [Fact]
        public void ValidateEmployee_WithDefaultBirthDay_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = default,
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Birth Day is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithFutureBirthDay_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = DateTime.Now.AddDays(1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Birth Day cannot be in the future", result.Errors);
        }

        [Theory]
        [InlineData("1990-01-01")]
        [InlineData("2000-12-31")]
        public void ValidateEmployee_WithValidBirthDay_PassesValidation(string birthDayString)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = DateTime.Parse(birthDayString),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.DoesNotContain("Birth Day is required", result.Errors);
            Assert.DoesNotContain("Birth Day cannot be in the future", result.Errors);
        }
        #endregion

        #region Occupation Validation Tests
        [Fact]
        public void ValidateEmployee_WithNullOccupation_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = null,
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Occupation is required", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithEmptyOccupation_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Occupation is required", result.Errors);
        }
        #endregion

        #region Sex Validation Tests
        [Fact]
        public void ValidateEmployee_WithNullSex_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = null
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Sex must be either Male or Female", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithEmptySex_ReturnsError()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = ""
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Sex must be either Male or Female", result.Errors);
        }

        [Theory]
        [InlineData("male")]
        [InlineData("female")]
        [InlineData("Other")]
        [InlineData("M")]
        public void ValidateEmployee_WithInvalidSex_ReturnsError(string invalidSex)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = invalidSex
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Sex must be either Male or Female", result.Errors);
        }

        [Theory]
        [InlineData("Male")]
        [InlineData("Female")]
        public void ValidateEmployee_WithValidSex_PassesValidation(string validSex)
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = validSex
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.DoesNotContain("Sex must be either Male or Female", result.Errors);
        }
        #endregion

        #region Integration Tests
        [Fact]
        public void ValidateEmployee_WithAllValidData_ReturnsValid()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithMultipleErrors_ReturnsAllErrors()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Phone = "123",
                BirthDay = DateTime.Now.AddDays(1),
                Occupation = "",
                Sex = "Other"
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("First Name is required", result.Errors);
            Assert.Contains("Last Name is required", result.Errors);
            Assert.Contains("Email format is invalid", result.Errors);
            Assert.Contains("Phone format is invalid", result.Errors);
            Assert.Contains("Birth Day cannot be in the future", result.Errors);
            Assert.Contains("Occupation is required", result.Errors);
            Assert.Contains("Sex must be either Male or Female", result.Errors);
        }

        [Fact]
        public void ValidateEmployee_WithProfileImageBase64_DoesNotValidateImage()
        {
            // Arrange - ProfileImage validation is not implemented, so any base64 is accepted
            var request = new CreateEmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male",
                ProfileImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
            };

            // Act
            var result = _validationService.ValidateEmployee(request);

            // Assert
            Assert.True(result.IsValid);
        }
        #endregion
    }
}
