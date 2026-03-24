namespace EmployeeManagementAPI.Models
{
    /// Represents an employee record in the system.
    public class Employee
    {
        // Primary key — auto incremented by the database.
        public int Id { get; set; }

        // Full name of the employee.
        public string Name { get; set; } = string.Empty;

        // Job title or position of the employee.
        public string Position { get; set; } = string.Empty;

        // Employee's salary.
        public decimal Salary { get; set; }

        // Foreign key — links this employee to a Department.
        public int DepartmentId { get; set; }

        // Navigation property — the related Department object.
        public Department? Department { get; set; }
    }
}
