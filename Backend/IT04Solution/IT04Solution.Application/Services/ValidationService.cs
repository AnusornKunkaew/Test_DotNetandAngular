using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IT04Solution.Application.Services
{
    public class ValidationService
    {
        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public List<string> Errors { get; set; } = new();
        }

        public ValidationResult ValidateEmployee(CreateEmployeeRequestDto request)
        {
            var result = new ValidationResult { IsValid = true };

            // Required fields
            if (string.IsNullOrWhiteSpace(request.FirstName))
                result.Errors.Add("First Name is required");

            if (string.IsNullOrWhiteSpace(request.LastName))
                result.Errors.Add("Last Name is required");

            if (string.IsNullOrWhiteSpace(request.Email))
                result.Errors.Add("Email is required");

            if (string.IsNullOrWhiteSpace(request.Phone))
                result.Errors.Add("Phone is required");

            if (string.IsNullOrWhiteSpace(request.Occupation))
                result.Errors.Add("Occupation is required");

            // Email format validation
            if (!string.IsNullOrWhiteSpace(request.Email) && !IsValidEmail(request.Email))
                result.Errors.Add("Email format is invalid");

            // Phone format validation
            if (!string.IsNullOrWhiteSpace(request.Phone) && !IsValidPhone(request.Phone))
                result.Errors.Add("Phone format is invalid");

            // BirthDay validation
            if (request.BirthDay == default)
                result.Errors.Add("Birth Day is required");
            else if (request.BirthDay > DateTime.Now)
                result.Errors.Add("Birth Day cannot be in the future");

            // Sex validation
            if (string.IsNullOrWhiteSpace(request.Sex) || (request.Sex != "Male" && request.Sex != "Female"))
                result.Errors.Add("Sex must be either Male or Female");

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            var digitsOnly = System.Text.RegularExpressions.Regex.Replace(phone, "[^0-9]", "");

            return digitsOnly.Length >= 9 && digitsOnly.Length <= 10;
        }
    }

    public class CreateEmployeeRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; } = string.Empty;
        public string? ProfileImageBase64 { get; set; }
        public string Sex { get; set; } = string.Empty;
    }
}
