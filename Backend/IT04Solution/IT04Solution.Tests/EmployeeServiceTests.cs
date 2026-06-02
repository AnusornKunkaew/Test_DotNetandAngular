using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT04Solution.Application.DTOs;
using IT04Solution.Application.Interfaces;
using IT04Solution.Application.Services;
using Moq;
using Xunit;

namespace IT04Solution.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepository;
        private readonly ValidationService _validationService;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _mockRepository = new Mock<IEmployeeRepository>();
            _validationService = new ValidationService();
            _employeeService = new EmployeeService(_mockRepository.Object, _validationService);
        }

        private CreateEmployeeRequest CreateValidEmployeeRequest()
        {
            return new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };
        }

        private EmployeeResponse CreateEmployeeResponse(int id = 1)
        {
            return new EmployeeResponse
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male",
                CreatedAt = DateTime.UtcNow
            };
        }

        #region CreateEmployeeAsync Tests
        [Fact]
        public async Task CreateEmployeeAsync_WithValidRequest_CallsRepository()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse();
            _mockRepository.Setup(r => r.CreateAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _employeeService.CreateEmployeeAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(response.Id, result.Id);
            Assert.Equal(response.FirstName, result.FirstName);
            _mockRepository.Verify(r => r.CreateAsync(request), Times.Once);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithValidRequest_ReturnsEmployeeResponse()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse();
            _mockRepository.Setup(r => r.CreateAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _employeeService.CreateEmployeeAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<EmployeeResponse>(result);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithInvalidFirstName_ThrowsArgumentException()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.FirstName = "";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.CreateEmployeeAsync(request));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithInvalidEmail_ThrowsArgumentException()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.Email = "invalid-email";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.CreateEmployeeAsync(request));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithInvalidPhone_ThrowsArgumentException()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.Phone = "123";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.CreateEmployeeAsync(request));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithFutureBirthDay_ThrowsArgumentException()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.BirthDay = DateTime.Now.AddDays(1);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.CreateEmployeeAsync(request));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithInvalidSex_ThrowsArgumentException()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.Sex = "Other";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.CreateEmployeeAsync(request));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithProfileImage_IncludesInRequest()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.ProfileImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";
            var response = CreateEmployeeResponse();
            _mockRepository.Setup(r => r.CreateAsync(request)).ReturnsAsync(response);

            // Act
            await _employeeService.CreateEmployeeAsync(request);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(It.Is<CreateEmployeeRequest>(
                req => req.ProfileImageBase64 == request.ProfileImageBase64)), Times.Once);
        }
        #endregion

        #region GetEmployeeByIdAsync Tests
        [Fact]
        public async Task GetEmployeeByIdAsync_WithValidId_ReturnsEmployee()
        {
            // Arrange
            var id = 1;
            var response = CreateEmployeeResponse(id);
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            var id = 999;
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((EmployeeResponse)null);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(id);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WithZeroId_CallsRepository()
        {
            // Arrange
            var id = 0;
            var response = new EmployeeResponse { Id = 0 };
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }
        #endregion

        #region GetAllEmployeesAsync Tests
        [Fact]
        public async Task GetAllEmployeesAsync_WithEmployeesInDatabase_ReturnsAllEmployees()
        {
            // Arrange
            var employees = new List<EmployeeResponse>
            {
                CreateEmployeeResponse(1),
                CreateEmployeeResponse(2),
                CreateEmployeeResponse(3)
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_WithNoEmployeesInDatabase_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<EmployeeResponse>());

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsCorrectEmployeeData()
        {
            // Arrange
            var employees = new List<EmployeeResponse>
            {
                new EmployeeResponse
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com"
                },
                new EmployeeResponse
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com"
                }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Jane", result[1].FirstName);
        }
        #endregion

        #region UpdateEmployeeAsync Tests
        [Fact]
        public async Task UpdateEmployeeAsync_WithValidRequest_CallsRepository()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse(id);
            _mockRepository.Setup(r => r.UpdateAsync(id, request)).ReturnsAsync(response);

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(id, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _mockRepository.Verify(r => r.UpdateAsync(id, request), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithInvalidFirstName_ThrowsArgumentException()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            request.FirstName = "";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.UpdateEmployeeAsync(id, request));
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithInvalidEmail_ThrowsArgumentException()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            request.Email = "invalid-email";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employeeService.UpdateEmployeeAsync(id, request));
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithValidRequest_ReturnsUpdatedEmployee()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse(id);
            _mockRepository.Setup(r => r.UpdateAsync(id, request)).ReturnsAsync(response);

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(id, request);

            // Assert
            Assert.IsType<EmployeeResponse>(result);
        }
        #endregion

        #region DeleteEmployeeAsync Tests
        [Fact]
        public async Task DeleteEmployeeAsync_WithValidId_CallsRepository()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(id);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_WithNonExistentId_ReturnsFalse()
        {
            // Arrange
            var id = 999;
            _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(id);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_CallsRepositoryDelete()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            await _employeeService.DeleteEmployeeAsync(id);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.Is<int>(x => x == id)), Times.Once);
        }
        #endregion

        #region Integration Tests
        [Fact]
        public async Task CreateThenGetEmployee_VerifiesFlowWorks()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse(1);
            _mockRepository.Setup(r => r.CreateAsync(createRequest)).ReturnsAsync(response);
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(response);

            // Act
            var createResult = await _employeeService.CreateEmployeeAsync(createRequest);
            var getResult = await _employeeService.GetEmployeeByIdAsync(createResult.Id);

            // Assert
            Assert.NotNull(createResult);
            Assert.NotNull(getResult);
            Assert.Equal(createResult.Id, getResult.Id);
        }

        [Fact]
        public async Task UpdateThenGetEmployee_VerifiesUpdatedData()
        {
            // Arrange
            var id = 1;
            var updateRequest = CreateValidEmployeeRequest();
            updateRequest.FirstName = "UpdatedName";
            var updatedResponse = CreateEmployeeResponse(id);
            updatedResponse.FirstName = "UpdatedName";
            
            _mockRepository.Setup(r => r.UpdateAsync(id, updateRequest)).ReturnsAsync(updatedResponse);
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(updatedResponse);

            // Act
            var updateResult = await _employeeService.UpdateEmployeeAsync(id, updateRequest);
            var getResult = await _employeeService.GetEmployeeByIdAsync(id);

            // Assert
            Assert.Equal("UpdatedName", updateResult.FirstName);
            Assert.Equal("UpdatedName", getResult.FirstName);
        }
        #endregion
    }
}
