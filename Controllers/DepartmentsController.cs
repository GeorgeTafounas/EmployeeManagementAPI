using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        /// Returns all departments from the database.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        /// Returns a simplified list of departments for UI dropdown — name only.
        [HttpGet("dropdown")]
        public async Task<ActionResult<IEnumerable<object>>> GetDropdown()
        {
            // Return only Id and Name for use in frontend dropdown components
            return await _context.Departments
                .Select(d => new { d.Id, d.Name })
                .ToListAsync<object>();
        }

        /// Returns a single department by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound(new { message = $"Department with ID {id} not found." });
            return department;
        }

        /// Returns all employees belonging to a specific department.
        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByDepartment(int id)
        {
            // Check department exists first
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound(new { message = $"Department with ID {id} not found." });

            var employees = await _context.Employees
                .Where(e => e.DepartmentId == id)
                .ToListAsync();

            return employees;
        }

        /// Creates a new department and saves it to the database.
        [HttpPost]
        public async Task<ActionResult<Department>> Create(Department department)
        {
            // Check for duplicate department name
            if (await _context.Departments.AnyAsync(d => d.Name == department.Name))
                return Conflict(new { message = "A department with this name already exists." });

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        /// Updates an existing department matched by ID.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Department department)
        {
            if (id != department.Id) return BadRequest(new { message = "ID in URL does not match ID in body." });

            // Check for duplicate department name on update
            if (await _context.Departments.AnyAsync(d => d.Name == department.Name && d.Id != id))
                return Conflict(new { message = "A department with this name already exists." });

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Departments.AnyAsync(e => e.Id == id))
                    return NotFound(new { message = $"Department with ID {id} not found." });
                throw;
            }

            return NoContent();
        }

        /// Deletes a department by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound(new { message = $"Department with ID {id} not found." });

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
