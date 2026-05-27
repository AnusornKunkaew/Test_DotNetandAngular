using System;
using System.Collections.Generic;

namespace IT04Solution.Application.DTOs
{
    public class CreateEmployeeRequest
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

    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; } = string.Empty;
        public string? ProfileImageBase64 { get; set; }
        public string Sex { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
