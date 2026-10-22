using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public int DeviceCategoryId { get; set; }
        public DeviceCategory? DeviceCategory { get; set; }

        public DeviceStatus Status { get; set; } = DeviceStatus.InUse;

        public DateTime EntryDate { get; set; } = DateTime.Today;

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
