using DeviceManagement.Models;

namespace DeviceManagement.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<DeviceCategory>> GetAllAsync();
        Task<DeviceCategory?> GetByIdAsync(int id);
        Task<bool> HasDevicesAsync(int id);
        Task AddAsync(DeviceCategory category);
        Task UpdateAsync(DeviceCategory category);
        Task DeleteAsync(int id);
    }
}
