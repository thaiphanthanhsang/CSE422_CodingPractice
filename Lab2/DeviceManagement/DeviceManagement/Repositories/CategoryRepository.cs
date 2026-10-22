using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DeviceManagementDbContext _context;

        public CategoryRepository(DeviceManagementDbContext context)
        {
            _context = context;
        }

        public Task<List<DeviceCategory>> GetAllAsync()
        {
            return _context.DeviceCategories
                .Include(c => c.Devices)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public Task<DeviceCategory?> GetByIdAsync(int id)
        {
            return _context.DeviceCategories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<bool> HasDevicesAsync(int id)
        {
            return _context.Devices.AnyAsync(d => d.DeviceCategoryId == id);
        }

        public async Task AddAsync(DeviceCategory category)
        {
            _context.DeviceCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeviceCategory category)
        {
            _context.DeviceCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.DeviceCategories.FindAsync(id);
            if (category == null) return;

            _context.DeviceCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
