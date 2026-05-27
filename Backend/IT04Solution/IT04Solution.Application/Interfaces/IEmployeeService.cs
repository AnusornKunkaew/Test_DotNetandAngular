using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT04Solution.Application.DTOs;

namespace IT04Solution.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
        Task<EmployeeResponse?> GetByIdAsync(int id);
        Task<List<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse> UpdateAsync(int id, CreateEmployeeRequest request);
        Task<bool> DeleteAsync(int id);
    }

    public interface IEmployeeService
    {
        Task<EmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request);
        Task<EmployeeResponse?> GetEmployeeByIdAsync(int id);
        Task<List<EmployeeResponse>> GetAllEmployeesAsync();
        Task<EmployeeResponse> UpdateEmployeeAsync(int id, CreateEmployeeRequest request);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
