using DeviceManagement.Models;
using DeviceManagement.Repositories;
using DeviceManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceManagement.Controllers
{
    public class DevicesController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public DevicesController(
            IDeviceRepository deviceRepository,
            ICategoryRepository categoryRepository,
            IEmployeeRepository employeeRepository)
        {
            _deviceRepository = deviceRepository;
            _categoryRepository = categoryRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index(string? keyword, DeviceStatus? status, int? categoryId)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var viewModel = new DeviceListViewModel
            {
                Devices = await _deviceRepository.SearchAsync(keyword, status, categoryId),
                Keyword = keyword,
                Status = status,
                CategoryId = categoryId,
                Categories = categories
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString(), c.Id == categoryId))
                    .ToList()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            return View(await BuildFormAsync(new DeviceFormViewModel()));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceFormViewModel form)
        {
            await ValidateCodeAsync(form);
            if (!ModelState.IsValid) return View(await BuildFormAsync(form));

            var device = new Device();
            ApplyForm(device, form);
            await _deviceRepository.AddAsync(device);
            TempData["Success"] = "Device created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) return NotFound();

            return View(await BuildFormAsync(ToForm(device)));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeviceFormViewModel form)
        {
            if (id != form.Id) return NotFound();

            await ValidateCodeAsync(form);
            if (!ModelState.IsValid) return View(await BuildFormAsync(form));

            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) return NotFound();

            ApplyForm(device, form);
            await _deviceRepository.UpdateAsync(device);
            TempData["Success"] = "Device updated.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            return device == null ? NotFound() : View(device);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _deviceRepository.DeleteAsync(id);
            TempData["Success"] = "Device deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateCodeAsync(DeviceFormViewModel form)
        {
            if (await _deviceRepository.CodeExistsAsync(form.Code, form.Id == 0 ? null : form.Id))
            {
                ModelState.AddModelError(nameof(form.Code), "This device code already exists.");
            }
        }

        private async Task<DeviceFormViewModel> BuildFormAsync(DeviceFormViewModel form)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();

            form.Categories = categories.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            form.Employees = employees.Select(e => new SelectListItem(e.FullName, e.Id.ToString()));
            return form;
        }

        private static DeviceFormViewModel ToForm(Device device)
        {
            return new DeviceFormViewModel
            {
                Id = device.Id,
                Name = device.Name,
                Code = device.Code,
                DeviceCategoryId = device.DeviceCategoryId,
                Status = device.Status,
                EntryDate = device.EntryDate,
                EmployeeId = device.EmployeeId
            };
        }

        private static void ApplyForm(Device device, DeviceFormViewModel form)
        {
            device.Name = form.Name;
            device.Code = form.Code;
            device.DeviceCategoryId = form.DeviceCategoryId!.Value;
            device.Status = form.Status;
            device.EntryDate = form.EntryDate;
            device.EmployeeId = form.EmployeeId;
            device.Employee = null;
            device.DeviceCategory = null;
        }
    }
}

