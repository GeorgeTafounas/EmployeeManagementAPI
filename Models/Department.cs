namespace EmployeeManagementAPI.Models
{
    /// Represents a department within the organisation.
    public class Department
    {
        // Primary key — auto incremented by the database.
        public int Id { get; set; }

        // Name of the department.
        public string Name { get; set; } = string.Empty;

        // Navigation property — collection of employees in this department.
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
