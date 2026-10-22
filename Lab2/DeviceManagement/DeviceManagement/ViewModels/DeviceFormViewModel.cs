using System.ComponentModel.DataAnnotations;
using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceManagement.ViewModels
{
    public class DeviceFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Device name is required.")]
        [MaxLength(100, ErrorMessage = "Device name cannot exceed 100 characters.")]
        [Display(Name = "Device name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device code is required.")]
        [MaxLength(50, ErrorMessage = "Device code cannot exceed 50 characters.")]
        [Display(Name = "Device code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device category is required.")]
        [Display(Name = "Category")]
        public int? DeviceCategoryId { get; set; }

        [Required(ErrorMessage = "Device status is required.")]
        public DeviceStatus Status { get; set; } = DeviceStatus.InUse;

        [Display(Name = "Date of entry")]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; } = DateTime.Today;

        [Display(Name = "Assigned employee")]
        public int? EmployeeId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    }
}
