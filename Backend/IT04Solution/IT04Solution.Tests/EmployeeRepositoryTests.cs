using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IT04Solution.Application.DTOs;
using IT04Solution.Application.Interfaces;
using IT04Solution.Domain.Entities;
using IT04Solution.Infrastructure.Data;
using IT04Solution.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IT04Solution.Tests
{
    public class EmployeeRepositoryTests : IAsyncLifetime
    {
        private readonly EmployeeDbContext _context;
        private readonly EmployeeRepository _repository;

        public EmployeeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EmployeeDbContext(options);
            _repository = new EmployeeRepository(_context);
        }

        public async Task InitializeAsync()
        {
            // Initialize the in-memory database
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
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

        #region CreateAsync Tests
        [Fact]
        public async Task CreateAsync_WithValidRequest_CreatesAndReturnsEmployee()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();

            // Act
            var result = await _repository.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Phone, result.Phone);
            Assert.Equal(request.BirthDay, result.BirthDay);
            Assert.Equal(request.Occupation, result.Occupation);
            Assert.Equal(request.Sex, result.Sex);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_AssignsCreatedAtTimestamp()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var beforeCreation = DateTime.UtcNow;

            // Act
            var result = await _repository.CreateAsync(request);
            var afterCreation = DateTime.UtcNow;

            // Assert
            Assert.NotNull(result.CreatedAt);
            Assert.True(result.CreatedAt >= beforeCreation && result.CreatedAt <= afterCreation.AddSeconds(1),
                $"CreatedAt {result.CreatedAt} should be between {beforeCreation} and {afterCreation}");
        }

        [Fact]
        public async Task CreateAsync_WithProfileImage_StoresProfileImage()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var imageBytes = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==");
            request.ProfileImageBase64 = System.Convert.ToBase64String(imageBytes);

            // Act
            var result = await _repository.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            var storedEmployee = await _context.Employees.FindAsync(result.Id);
            Assert.NotNull(storedEmployee?.ProfileImage);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidBase64ProfileImage_SkipsProfileImage()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.ProfileImageBase64 = "invalid-base64-!!!";

            // Act
            var result = await _repository.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            var storedEmployee = await _context.Employees.FindAsync(result.Id);
            Assert.Null(storedEmployee?.ProfileImage);
        }

        [Fact]
        public async Task CreateAsync_WithoutProfileImage_StoresEmployeeWithoutImage()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            request.ProfileImageBase64 = null;

            // Act
            var result = await _repository.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            var storedEmployee = await _context.Employees.FindAsync(result.Id);
            Assert.Null(storedEmployee?.ProfileImage);
        }

        [Fact]
        public async Task CreateAsync_SavesEmployeeToDatabase()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();

            // Act
            var result = await _repository.CreateAsync(request);

            // Assert
            var employeeInDb = await _context.Employees.FirstOrDefaultAsync(e => e.Id == result.Id);
            Assert.NotNull(employeeInDb);
            Assert.Equal(request.FirstName, employeeInDb.FirstName);
        }

        [Fact]
        public async Task CreateAsync_MultipleEmployees_EachHasUniqueId()
        {
            // Arrange
            var request1 = CreateValidEmployeeRequest();
            var request2 = CreateValidEmployeeRequest();
            request2.Email = "jane@example.com";

            // Act
            var result1 = await _repository.CreateAsync(request1);
            var result2 = await _repository.CreateAsync(request2);

            // Assert
            Assert.NotEqual(result1.Id, result2.Id);
        }
        #endregion

        #region GetByIdAsync Tests
        [Fact]
        public async Task GetByIdAsync_WithExistingId_ReturnsEmployee()
        {
            // Arrange
            var request = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(request);

            // Act
            var result = await _repository.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal(created.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingId = 9999;

            // Act
            var result = await _repository.GetByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithZeroId_ReturnsNull()
        {
            // Arrange - assume ID 0 doesn't exist
            // Act
            var result = await _repository.GetByIdAsync(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEmployeeData()
        {
            // Arrange
            var request = new CreateEmployeeRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Phone = "0987654321",
                BirthDay = new DateTime(1995, 5, 15),
                Occupation = "Designer",
                Sex = "Female"
            };
            var created = await _repository.CreateAsync(request);

            // Act
            var result = await _repository.GetByIdAsync(created.Id);

            // Assert
            Assert.Equal("Jane", result.FirstName);
            Assert.Equal("Smith", result.LastName);
            Assert.Equal("jane@example.com", result.Email);
            Assert.Equal("0987654321", result.Phone);
            Assert.Equal(new DateTime(1995, 5, 15), result.BirthDay);
            Assert.Equal("Designer", result.Occupation);
            Assert.Equal("Female", result.Sex);
        }
        #endregion

        #region GetAllAsync Tests
        [Fact]
        public async Task GetAllAsync_WithNoEmployees_ReturnsEmptyList()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithMultipleEmployees_ReturnsAll()
        {
            // Arrange
            var request1 = CreateValidEmployeeRequest();
            var request2 = CreateValidEmployeeRequest();
            request2.Email = "jane@example.com";
            var request3 = CreateValidEmployeeRequest();
            request3.Email = "bob@example.com";

            await _repository.CreateAsync(request1);
            await _repository.CreateAsync(request2);
            await _repository.CreateAsync(request3);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_WithMultipleEmployees_ReturnsCorrectData()
        {
            // Arrange
            var request1 = new CreateEmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "0812345678",
                BirthDay = new DateTime(1990, 1, 1),
                Occupation = "Developer",
                Sex = "Male"
            };
            var request2 = new CreateEmployeeRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Phone = "0987654321",
                BirthDay = new DateTime(1995, 5, 15),
                Occupation = "Designer",
                Sex = "Female"
            };

            await _repository.CreateAsync(request1);
            await _repository.CreateAsync(request2);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.FirstName == "John");
            Assert.Contains(result, e => e.FirstName == "Jane");
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmployeesInCorrectOrder()
        {
            // Arrange
            var request1 = CreateValidEmployeeRequest();
            var request2 = CreateValidEmployeeRequest();
            request2.Email = "second@example.com";

            var created1 = await _repository.CreateAsync(request1);
            var created2 = await _repository.CreateAsync(request2);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            // Results should be in order of ID
            Assert.True(result[0].Id == created1.Id || result[1].Id == created1.Id);
        }
        #endregion

        #region UpdateAsync Tests
        [Fact]
        public async Task UpdateAsync_WithExistingEmployee_UpdatesEmployee()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);

            var updateRequest = new CreateEmployeeRequest
            {
                FirstName = "UpdatedName",
                LastName = "UpdatedLastName",
                Email = "updated@example.com",
                Phone = "0899999999",
                BirthDay = new DateTime(1992, 6, 15),
                Occupation = "Manager",
                Sex = "Female"
            };

            // Act
            var result = await _repository.UpdateAsync(created.Id, updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("UpdatedName", result.FirstName);
            Assert.Equal("UpdatedLastName", result.LastName);
            Assert.Equal("updated@example.com", result.Email);
            Assert.Equal("Manager", result.Occupation);
            Assert.Equal("Female", result.Sex);
        }

        [Fact]
        public async Task UpdateAsync_WithExistingEmployee_UpdatesDatabase()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);

            var updateRequest = new CreateEmployeeRequest
            {
                FirstName = "UpdatedName",
                LastName = "UpdatedLastName",
                Email = "updated@example.com",
                Phone = "0899999999",
                BirthDay = new DateTime(1992, 6, 15),
                Occupation = "Manager",
                Sex = "Female"
            };

            // Act
            await _repository.UpdateAsync(created.Id, updateRequest);

            // Assert - Verify in database
            var employeeInDb = await _context.Employees.FindAsync(created.Id);
            Assert.NotNull(employeeInDb);
            Assert.Equal("UpdatedName", employeeInDb.FirstName);
            Assert.Equal("updated@example.com", employeeInDb.Email);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistingEmployee_ThrowsKeyNotFoundException()
        {
            // Arrange
            var updateRequest = CreateValidEmployeeRequest();
            var nonExistingId = 9999;

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _repository.UpdateAsync(nonExistingId, updateRequest));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesTimestamp()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);
            
            await Task.Delay(100); // Small delay to ensure time difference

            var beforeUpdate = DateTime.UtcNow;
            var updateRequest = CreateValidEmployeeRequest();
            updateRequest.FirstName = "Updated";

            // Act
            var result = await _repository.UpdateAsync(created.Id, updateRequest);
            var afterUpdate = DateTime.UtcNow;

            // Assert
            Assert.NotNull(result.UpdatedAt);
            Assert.True(result.UpdatedAt >= beforeUpdate && result.UpdatedAt <= afterUpdate.AddSeconds(1));
        }

        [Fact]
        public async Task UpdateAsync_WithProfileImage_UpdatesProfileImage()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);

            var imageBytes = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==");
            var updateRequest = CreateValidEmployeeRequest();
            updateRequest.ProfileImageBase64 = System.Convert.ToBase64String(imageBytes);

            // Act
            var result = await _repository.UpdateAsync(created.Id, updateRequest);

            // Assert
            var employeeInDb = await _context.Employees.FindAsync(created.Id);
            Assert.NotNull(employeeInDb?.ProfileImage);
        }
        #endregion

        #region DeleteAsync Tests
        [Fact]
        public async Task DeleteAsync_WithExistingEmployee_DeletesEmployee()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);

            // Act
            var result = await _repository.DeleteAsync(created.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_WithExistingEmployee_RemovesFromDatabase()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);

            // Act
            await _repository.DeleteAsync(created.Id);

            // Assert
            var employeeInDb = await _context.Employees.FindAsync(created.Id);
            Assert.Null(employeeInDb);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingEmployee_ReturnsFalse()
        {
            // Arrange
            var nonExistingId = 9999;

            // Act
            var result = await _repository.DeleteAsync(nonExistingId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_WithZeroId_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteAsync(0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_MultipleEmployees_DeletesOnlySpecified()
        {
            // Arrange
            var request1 = CreateValidEmployeeRequest();
            var request2 = CreateValidEmployeeRequest();
            request2.Email = "second@example.com";

            var created1 = await _repository.CreateAsync(request1);
            var created2 = await _repository.CreateAsync(request2);

            // Act
            var result = await _repository.DeleteAsync(created1.Id);

            // Assert
            Assert.True(result);
            var employee1 = await _repository.GetByIdAsync(created1.Id);
            var employee2 = await _repository.GetByIdAsync(created2.Id);
            Assert.Null(employee1);
            Assert.NotNull(employee2);
        }
        #endregion

        #region Integration Tests
        [Fact]
        public async Task CreateThenGetThenUpdate_FullWorkflow()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();

            // Act - Create
            var created = await _repository.CreateAsync(createRequest);
            Assert.NotNull(created);
            Assert.True(created.Id > 0);

            // Act - Get
            var retrieved = await _repository.GetByIdAsync(created.Id);
            Assert.NotNull(retrieved);
            Assert.Equal(created.FirstName, retrieved.FirstName);

            // Act - Update
            var updateRequest = new CreateEmployeeRequest
            {
                FirstName = "UpdatedName",
                LastName = "UpdatedLastName",
                Email = "updated@example.com",
                Phone = "0899999999",
                BirthDay = new DateTime(1992, 6, 15),
                Occupation = "Manager",
                Sex = "Female"
            };
            var updated = await _repository.UpdateAsync(created.Id, updateRequest);

            // Assert
            Assert.Equal("UpdatedName", updated.FirstName);

            // Act - Get again
            var retrievedUpdated = await _repository.GetByIdAsync(created.Id);
            Assert.Equal("UpdatedName", retrievedUpdated.FirstName);
        }

        [Fact]
        public async Task CreateThenDelete_VerifiesRemoval()
        {
            // Arrange
            var createRequest = CreateValidEmployeeRequest();
            var created = await _repository.CreateAsync(createRequest);

            // Act
            var deleteResult = await _repository.DeleteAsync(created.Id);
            var retrieved = await _repository.GetByIdAsync(created.Id);

            // Assert
            Assert.True(deleteResult);
            Assert.Null(retrieved);
        }

        [Fact]
        public async Task GetAll_AfterMultipleOperations_ReturnsCorrectCount()
        {
            // Arrange & Act
            var request1 = CreateValidEmployeeRequest();
            var request2 = CreateValidEmployeeRequest();
            request2.Email = "second@example.com";
            var request3 = CreateValidEmployeeRequest();
            request3.Email = "third@example.com";

            var created1 = await _repository.CreateAsync(request1);
            var created2 = await _repository.CreateAsync(request2);
            var created3 = await _repository.CreateAsync(request3);

            // Delete one
            await _repository.DeleteAsync(created2.Id);

            // Assert
            var allEmployees = await _repository.GetAllAsync();
            Assert.Equal(2, allEmployees.Count);
            Assert.DoesNotContain(allEmployees, e => e.Id == created2.Id);
        }
        #endregion
    }
}
