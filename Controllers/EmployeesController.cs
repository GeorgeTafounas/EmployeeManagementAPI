using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        /// Returns all employees including their related department info.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }

        /// Returns the top N employees with the highest salary.
        [HttpGet("highest-salary")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetHighestSalary([FromQuery] int top = 2)
        {
            // Order by salary descending and take the top N results
            var employees = await _context.Employees
                .Include(e => e.Department)
                .OrderByDescending(e => e.Salary)
                .Take(top)
                .ToListAsync();

            return employees;
        }

        /// Returns the average salary of employees in a specific department.
        [HttpGet("average-salary/{departmentId}")]
        public async Task<IActionResult> GetAverageSalary(int departmentId)
        {
            // Check department exists
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
                return NotFound(new { message = $"Department with ID {departmentId} not found." });

            var averageSalary = await _context.Employees
                .Where(e => e.DepartmentId == departmentId)
                .AverageAsync(e => (double?)e.Salary) ?? 0;

            return Ok(new { departmentId, averageSalary });
        }

        /// Returns employees whose salary falls within the specified range.
        [HttpGet("salary-range")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetBySalaryRange(
            [FromQuery] decimal min,
            [FromQuery] decimal max)
        {
            if (min > max)
                return BadRequest(new { message = "Minimum salary cannot be greater than maximum salary." });

            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Salary >= min && e.Salary <= max)
                .ToListAsync();

            return employees;
        }

        /// Returns a single employee by ID including their department.
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound(new { message = $"Employee with ID {id} not found." });
            return employee;
        }

        /// Creates a new employee linked to an existing department.
        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            // Check department exists
            if (!await _context.Departments.AnyAsync(d => d.Id == employee.DepartmentId))
                return BadRequest(new { message = $"Department with ID {employee.DepartmentId} does not exist." });

            // Check for duplicate email
            if (await _context.Employees.AnyAsync(e => e.Email == employee.Email))
                return Conflict(new { message = "An employee with this email already exists." });

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        /// Returns an existing employee record matched by ID.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest(new { message = "ID in URL does not match ID in body." });

            // Check department exists
            if (!await _context.Departments.AnyAsync(d => d.Id == employee.DepartmentId))
                return BadRequest(new { message = $"Department with ID {employee.DepartmentId} does not exist." });

            // Check for duplicate email on update
            if (await _context.Employees.AnyAsync(e => e.Email == employee.Email && e.Id != id))
                return Conflict(new { message = "An employee with this email already exists." });

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Employees.AnyAsync(e => e.Id == id))
                    return NotFound(new { message = $"Employee with ID {id} not found." });
                throw;
            }

            return NoContent();
        }

        /// Deletes an employee by their ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound(new { message = $"Employee with ID {id} not found." });

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
