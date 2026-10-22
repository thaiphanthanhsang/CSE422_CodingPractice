using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DeviceManagementDbContext _context;

        public EmployeeRepository(DeviceManagementDbContext context)
        {
            _context = context;
        }

        public Task<List<Employee>> GetAllAsync()
        {
            return _context.Employees
                .Include(u => u.Devices)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public Task<Employee?> GetByIdAsync(int id)
        {
            return _context.Employees.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return _context.Employees.AnyAsync(u => u.Email == email && u.Id != excludeId);
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}


