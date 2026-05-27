using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT04Solution.Application.DTOs;
using IT04Solution.Application.Interfaces;

namespace IT04Solution.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly ValidationService _validationService;

        public EmployeeService(IEmployeeRepository repository, ValidationService validationService)
        {
            _repository = repository;
            _validationService = validationService;
        }

        public async Task<EmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var validationResult = _validationService.ValidateEmployee(new CreateEmployeeRequestDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                BirthDay = request.BirthDay,
                Occupation = request.Occupation,
                ProfileImageBase64 = request.ProfileImageBase64,
                Sex = request.Sex
            });

            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join(", ", validationResult.Errors));

            return await _repository.CreateAsync(request);
        }

        public async Task<EmployeeResponse?> GetEmployeeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<EmployeeResponse>> GetAllEmployeesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<EmployeeResponse> UpdateEmployeeAsync(int id, CreateEmployeeRequest request)
        {
            var validationResult = _validationService.ValidateEmployee(new CreateEmployeeRequestDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                BirthDay = request.BirthDay,
                Occupation = request.Occupation,
                ProfileImageBase64 = request.ProfileImageBase64,
                Sex = request.Sex
            });

            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join(", ", validationResult.Errors));

            return await _repository.UpdateAsync(id, request);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
