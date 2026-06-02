using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IT04Solution.Application.DTOs;
using IT04Solution.Application.Interfaces;
using IT04Solution.Domain.Entities;
using IT04Solution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IT04Solution.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
        {
            var now = DateTime.UtcNow;
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                BirthDay = request.BirthDay,
                Occupation = request.Occupation,
                Sex = request.Sex,
                CreatedAt = now,
                UpdatedAt = now
            };

            if (!string.IsNullOrEmpty(request.ProfileImageBase64))
            {
                try
                {
                    employee.ProfileImage = Convert.FromBase64String(request.ProfileImageBase64);
                }
                catch
                {
                    // Invalid base64, skip
                }
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return MapToResponse(employee);
        }

        public async Task<EmployeeResponse?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee != null ? MapToResponse(employee) : null;
        }

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees.Select(MapToResponse).ToList();
        }

        public async Task<EmployeeResponse> UpdateAsync(int id, CreateEmployeeRequest request)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {id} not found");

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Email = request.Email;
            employee.Phone = request.Phone;
            employee.BirthDay = request.BirthDay;
            employee.Occupation = request.Occupation;
            employee.Sex = request.Sex;
            employee.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(request.ProfileImageBase64))
            {
                try
                {
                    employee.ProfileImage = Convert.FromBase64String(request.ProfileImageBase64);
                }
                catch
                {
                    // Invalid base64, skip
                }
            }

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return MapToResponse(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        private EmployeeResponse MapToResponse(Employee employee)
        {
            return new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                BirthDay = employee.BirthDay,
                Occupation = employee.Occupation,
                Sex = employee.Sex,
                ProfileImageBase64 = employee.ProfileImage != null ? Convert.ToBase64String(employee.ProfileImage) : null,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }
    }
}
