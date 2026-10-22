using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceManagement.ViewModels
{
    public class DeviceListViewModel
    {
        public List<Device> Devices { get; set; } = new();
        public string? Keyword { get; set; }
        public DeviceStatus? Status { get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();
    }
}
