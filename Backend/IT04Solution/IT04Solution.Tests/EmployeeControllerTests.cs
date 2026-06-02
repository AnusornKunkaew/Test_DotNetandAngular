using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT04Solution.Api.Controllers;
using IT04Solution.Application.DTOs;
using IT04Solution.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IT04Solution.Tests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly Mock<ILogger<EmployeeController>> _mockLogger;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockLogger = new Mock<ILogger<EmployeeController>>();
            _controller = new EmployeeController(_mockEmployeeService.Object, _mockLogger.Object);
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

        #region CreateEmployee Tests
        [Fact]
        public async Task CreateEmployee_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse();
            _mockEmployeeService.Setup(s => s.CreateEmployeeAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateEmployee_WithValidRequest_CallsService()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse();
            _mockEmployeeService.Setup(s => s.CreateEmployeeAsync(request)).ReturnsAsync(response);

            // Act
            await _controller.CreateEmployee(request);

            // Assert
            _mockEmployeeService.Verify(s => s.CreateEmployeeAsync(request), Times.Once);
        }

        [Fact]
        public async Task CreateEmployee_WithValidRequest_ReturnsOkStatusCode()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse();
            _mockEmployeeService.Setup(s => s.CreateEmployeeAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateEmployee_WithValidationError_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateEmployeeRequest { FirstName = "" };
            _mockEmployeeService.Setup(s => s.CreateEmployeeAsync(request))
                .ThrowsAsync(new ArgumentException("First Name is required"));

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateEmployee_WithGeneralException_ReturnsInternalServerError()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            _mockEmployeeService.Setup(s => s.CreateEmployeeAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region GetEmployee Tests
        [Fact]
        public async Task GetEmployee_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var response = CreateEmployeeResponse(id);
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetEmployee(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEmployee_WithValidId_CallsService()
        {
            // Arrange
            var id = 1;
            var response = CreateEmployeeResponse(id);
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(id)).ReturnsAsync(response);

            // Act
            await _controller.GetEmployee(id);

            // Assert
            _mockEmployeeService.Verify(s => s.GetEmployeeByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_WithValidId_ReturnsOkStatusCode()
        {
            // Arrange
            var id = 1;
            var response = CreateEmployeeResponse(id);
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetEmployee(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetEmployee_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = 9999;
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(id)).ReturnsAsync((EmployeeResponse)null);

            // Act
            var result = await _controller.GetEmployee(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetEmployee_WithNonExistentId_CallsService()
        {
            // Arrange
            var id = 9999;
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(id)).ReturnsAsync((EmployeeResponse)null);

            // Act
            await _controller.GetEmployee(id);

            // Assert
            _mockEmployeeService.Verify(s => s.GetEmployeeByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_WithGeneralException_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            _mockEmployeeService.Setup(s => s.GetEmployeeByIdAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetEmployee(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region GetAllEmployees Tests
        [Fact]
        public async Task GetAllEmployees_WithMultipleEmployees_ReturnsOkResult()
        {
            // Arrange
            var employees = new List<EmployeeResponse>
            {
                CreateEmployeeResponse(1),
                CreateEmployeeResponse(2),
                CreateEmployeeResponse(3)
            };
            _mockEmployeeService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllEmployees_WithMultipleEmployees_CallsService()
        {
            // Arrange
            var employees = new List<EmployeeResponse>
            {
                CreateEmployeeResponse(1),
                CreateEmployeeResponse(2)
            };
            _mockEmployeeService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            await _controller.GetAllEmployees();

            // Assert
            _mockEmployeeService.Verify(s => s.GetAllEmployeesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployees_WithMultipleEmployees_ReturnsOkStatusCode()
        {
            // Arrange
            var employees = new List<EmployeeResponse>
            {
                CreateEmployeeResponse(1),
                CreateEmployeeResponse(2)
            };
            _mockEmployeeService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllEmployees_WithNoEmployees_ReturnsOkResult()
        {
            // Arrange
            _mockEmployeeService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(new List<EmployeeResponse>());

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllEmployees_WithGeneralException_ReturnsInternalServerError()
        {
            // Arrange
            _mockEmployeeService.Setup(s => s.GetAllEmployeesAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region UpdateEmployee Tests
        [Fact]
        public async Task UpdateEmployee_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse(id);
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(id, request)).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateEmployee(id, request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_WithValidRequest_CallsService()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse(id);
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(id, request)).ReturnsAsync(response);

            // Act
            await _controller.UpdateEmployee(id, request);

            // Assert
            _mockEmployeeService.Verify(s => s.UpdateEmployeeAsync(id, request), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_WithValidRequest_ReturnsOkStatusCode()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            var response = CreateEmployeeResponse(id);
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(id, request)).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateEmployee(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = 9999;
            var request = CreateValidEmployeeRequest();
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(id, request))
                .ThrowsAsync(new KeyNotFoundException($"Employee with ID {id} not found"));

            // Act
            var result = await _controller.UpdateEmployee(id, request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_WithValidationError_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var request = new CreateEmployeeRequest { FirstName = "" };
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(id, request))
                .ThrowsAsync(new ArgumentException("First Name is required"));

            // Act
            var result = await _controller.UpdateEmployee(id, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_WithGeneralException_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            var request = CreateValidEmployeeRequest();
            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(id, request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateEmployee(id, request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion

        #region DeleteEmployee Tests
        [Fact]
        public async Task DeleteEmployee_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WithValidId_CallsService()
        {
            // Arrange
            var id = 1;
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(id)).ReturnsAsync(true);

            // Act
            await _controller.DeleteEmployee(id);

            // Assert
            _mockEmployeeService.Verify(s => s.DeleteEmployeeAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_WithValidId_ReturnsOkStatusCode()
        {
            // Arrange
            var id = 1;
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var id = 9999;
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_WithNonExistentId_CallsService()
        {
            // Arrange
            var id = 9999;
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(id)).ReturnsAsync(false);

            // Act
            await _controller.DeleteEmployee(id);

            // Assert
            _mockEmployeeService.Verify(s => s.DeleteEmployeeAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_WithGeneralException_ReturnsInternalServerError()
        {
            // Arrange
            var id = 1;
            _mockEmployeeService.Setup(s => s.DeleteEmployeeAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        #endregion
    }
}
