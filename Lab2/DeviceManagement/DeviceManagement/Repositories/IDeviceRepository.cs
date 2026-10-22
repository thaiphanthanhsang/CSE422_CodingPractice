using DeviceManagement.Models;

namespace DeviceManagement.Repositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> SearchAsync(string? keyword, DeviceStatus? status, int? categoryId);
        Task<Device?> GetByIdAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task AddAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
    }
}
