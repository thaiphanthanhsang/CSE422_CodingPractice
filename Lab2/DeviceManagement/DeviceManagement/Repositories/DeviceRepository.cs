using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviceManagementDbContext _context;

        public DeviceRepository(DeviceManagementDbContext context)
        {
            _context = context;
        }

        public Task<List<Device>> SearchAsync(string? keyword, DeviceStatus? status, int? categoryId)
        {
            var query = _context.Devices
                .Include(d => d.DeviceCategory)
                .Include(d => d.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var term = keyword.Trim();
                query = query.Where(d => d.Name.Contains(term) || d.Code.Contains(term));
            }

            if (status.HasValue)
            {
                query = query.Where(d => d.Status == status.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(d => d.DeviceCategoryId == categoryId.Value);
            }

            return query.OrderBy(d => d.DeviceCategory!.Name).ThenBy(d => d.Name).ToListAsync();
        }

        public Task<Device?> GetByIdAsync(int id)
        {
            return _context.Devices
                .Include(d => d.DeviceCategory)
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            return _context.Devices.AnyAsync(d => d.Code == code && d.Id != excludeId);
        }

        public async Task AddAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }
    }
}

