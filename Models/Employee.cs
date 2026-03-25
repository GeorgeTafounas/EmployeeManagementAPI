using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models
{
    /// Represents an employee record in the system.
    public class Employee
    {
        // Primary key — auto incremented by the database.
        public int Id { get; set; }

        // First name of the employee — required field.
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        // Last name of the employee — required field.
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        // Email address — required, must be unique and valid format.
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be a valid format.")]
        public string Email { get; set; } = string.Empty;

        // Employee salary — required field.
        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value.")]
        public decimal Salary { get; set; }

        // Foreign key — links this employee to a Department.

        [Required(ErrorMessage = "DepartmentId is required.")]
        public int DepartmentId { get; set; }

        // Navigation property — the related Department object.
        public Department? Department { get; set; }
    }
}
