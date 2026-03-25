using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models
{
    /// Represents a department within the organisation.
    public class Department
    {
        // Primary key — auto incremented by the database.
        public int Id { get; set; }

        // Name of the department — required and must be unique.
        [Required(ErrorMessage = "Department name is required.")]
        public string Name { get; set; } = string.Empty;

        // Physical office location of the department — required field.
        [Required(ErrorMessage = "Office location is required.")]
        public string OfficeLocation { get; set; } = string.Empty;

        // Navigation property — collection of employees in this department.
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
