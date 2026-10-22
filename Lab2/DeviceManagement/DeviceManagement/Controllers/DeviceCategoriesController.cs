using DeviceManagement.Models;
using DeviceManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Controllers
{
    public class DeviceCategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeviceCategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepository.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View(new DeviceCategory());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceCategory category)
        {
            if (!ModelState.IsValid) return View(category);

            await _categoryRepository.AddAsync(category);
            TempData["Success"] = "Category created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? NotFound() : View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeviceCategory category)
        {
            if (id != category.Id) return NotFound();
            if (!ModelState.IsValid) return View(category);

            await _categoryRepository.UpdateAsync(category);
            TempData["Success"] = "Category updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? NotFound() : View(category);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _categoryRepository.HasDevicesAsync(id))
            {
                TempData["Error"] = "Cannot delete a category that still has devices.";
                return RedirectToAction(nameof(Index));
            }

            await _categoryRepository.DeleteAsync(id);
            TempData["Success"] = "Category deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
