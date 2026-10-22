using DeviceManagement.Models;
using DeviceManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _employeeRepository.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View(new Employee());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            await ValidateEmailAsync(employee);
            if (!ModelState.IsValid) return View(employee);

            await _employeeRepository.AddAsync(employee);
            TempData["Success"] = "Employee created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee == null ? NotFound() : View(employee);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id) return NotFound();

            await ValidateEmailAsync(employee);
            if (!ModelState.IsValid) return View(employee);

            await _employeeRepository.UpdateAsync(employee);
            TempData["Success"] = "Employee updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee == null ? NotFound() : View(employee);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            TempData["Success"] = "Employee deleted. Their devices are now unassigned.";
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateEmailAsync(Employee employee)
        {
            if (await _employeeRepository.EmailExistsAsync(employee.Email, employee.Id == 0 ? null : employee.Id))
            {
                ModelState.AddModelError(nameof(employee.Email), "This email is already in use.");
            }
        }
    }
}



