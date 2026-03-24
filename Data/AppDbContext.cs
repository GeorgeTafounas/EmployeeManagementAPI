using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Data
{
    /// Entity Framework Core database context.
    /// Manages the connection between the application and SQL Server.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Maps to the Departments table in SQL Server.
        public DbSet<Department> Departments { get; set; }

        // Maps to the Employees table in SQL Server.
        public DbSet<Employee> Employees { get; set; }
    }
}
